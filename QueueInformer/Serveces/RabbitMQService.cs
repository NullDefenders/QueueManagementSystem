using QueueService.DTO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqSse.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;


namespace RabbitMqSse.Services;

public class RabbitMQService : IAsyncDisposable
{
    private readonly ILogger<RabbitMQService> _logger;
    private readonly ConnectionFactory _connectionFactory;
    private readonly IConfiguration _configuration;

    private IConnection _connection;
    private IChannel _channel;
    private readonly string _queueName;
    private AsyncEventingBasicConsumer? _consumer;

    public event EventHandler<QueueDTO>? MessageReceived;

    public RabbitMQService(ILogger<RabbitMQService> logger, IConfiguration configuration)
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
        _queueName = configuration["Queues:Queue"] ?? "Queue";
  }

  public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
  {
    _connection = await _connectionFactory.CreateConnectionAsync();
    _channel = await _connection.CreateChannelAsync();


    // Объявляем очередь (если не существует)
    await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken
            );

    var consumer = new AsyncEventingBasicConsumer(_channel);


    consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<QueueDTO>(messageJson);

                if (message != null)
                {
                    MessageReceived?.Invoke(this, message);

                    // Подтверждаем обработку сообщения
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, cancellationToken);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: consumer);
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