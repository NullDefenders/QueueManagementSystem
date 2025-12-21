using Newtonsoft.Json;
using QueueService.DTO;
using QueueService.Factory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace QueueService.Services;

public class RabbitMQService : IAsyncDisposable
{
  private readonly ILogger<RabbitMQService> _logger;
  private readonly IConfiguration _configuration;

  private readonly ConnectionFactory _connectionFactory;
  private readonly MessageProcessorFactory _processorFactory;
  private IConnection _connection;
  private IChannel _channel;


  public RabbitMQService(ILogger<RabbitMQService> logger, IConfiguration configuration, MessageProcessorFactory processorFactory)
  {
    _logger = logger;
    _configuration = configuration;
    var rabbitConfig = configuration.GetSection("RabbitMQ");

    _connectionFactory = new ConnectionFactory()
    {
      HostName = rabbitConfig["HostName"] ?? "localhost",
      Port = int.Parse(rabbitConfig["Port"] ?? "5672"),
      UserName = rabbitConfig["UserName"] ?? "guest",
      Password = rabbitConfig["Password"] ?? "guest",
      VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
    };
    _processorFactory = processorFactory;
  }

  public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      _connection = await _connectionFactory.CreateConnectionAsync();
      _channel = await _connection.CreateChannelAsync();
      await _channel.BasicQosAsync(0, 1, false);

      List<string> queues = new List<string>();
      queues.Add(_configuration["Queues:WindowsQueue"]);
      queues.Add(_configuration["Queues:TalonQueue"]);

      foreach (var queueName in queues)
      {
        await SetupQueueConsumer(queueName, cancellationToken);
      }

      _logger.LogInformation("Successfully subscribed to all queues");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error starting RabbitMQ consumers");
      throw;
    }
  }

  private async Task SetupQueueConsumer(string queueName, CancellationToken cancellationToken)
  {
    await _channel.ExchangeDeclareAsync(
        exchange: queueName,
        type: ExchangeType.Fanout,
        durable: true,
        autoDelete: false
    );

    await _channel.QueueDeclareAsync(
        queue: $"{queueName}-QueueService",
        durable: true,
        exclusive: true,
        autoDelete: false,
        cancellationToken: cancellationToken
    );

    await _channel.QueueBindAsync(
        queue: $"{queueName}-QueueService",
        exchange: queueName,
        routingKey: ""
    );

    var processor = _processorFactory.CreateProcessor(queueName);
    var consumer = new AsyncEventingBasicConsumer(_channel);

    consumer.ReceivedAsync += async (sender, ea) =>
    {
      await ProcessMessageAsync(ea, queueName, processor);

    };

    await _channel.BasicConsumeAsync($"{queueName}-QueueService", false, consumer);
    _logger.LogInformation("Subscribed to queue: {QueueName}", $"{queueName}-QueueService");
  }

  private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, string queueName, IMessageProcessor processor)
  {
    try
    {
      var message = Encoding.UTF8.GetString(ea.Body.ToArray());
      _logger.LogInformation($"Processing: {message}");

      await processor.ProcessAsync(message);

      await _channel.BasicAckAsync(ea.DeliveryTag, false);
    }
    catch (Exception ex)
    {
      _logger.LogInformation($"Error: {ex.Message}");
      await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
    }
  }

 public async Task SendQueue(QueueDTO queue)
  {
    await _channel.ExchangeDeclareAsync(
        exchange: _configuration["Queues:Queue"],
        type: ExchangeType.Fanout,
        durable: true,
        autoDelete: false
    );

    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(queue));

    _channel.BasicReturnAsync += async (sender, args) =>
    {
      _logger.LogWarning($"Message returned: {args.ReplyText}");
      _logger.LogWarning($"Body: {Encoding.UTF8.GetString(args.Body.Span)}");
      await Task.CompletedTask;
    };

    var properties = new BasicProperties
    {
      Persistent = true, // сообщение сохраняется на диск
      MessageId = Guid.NewGuid().ToString(),
      ContentType = "application/json",
      ContentEncoding = "utf-8"
    };

    await _channel.BasicPublishAsync(
      exchange: _configuration["Queues:Queue"],
      routingKey: "",
      mandatory: false,
      basicProperties: properties,
      body: body
    );
  }


  public async ValueTask DisposeAsync()
  {
    if (_channel?.IsOpen == true)
    {
      await _channel.CloseAsync();
      _channel.Dispose();
    }

    if (_connection?.IsOpen == true)
    {
      await _connection.CloseAsync();
      _connection.Dispose();
    }
  }
}