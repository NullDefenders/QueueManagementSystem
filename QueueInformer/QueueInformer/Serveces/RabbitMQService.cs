using QueueInformer.Models;
using QueueInformer.Serveces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;


namespace QueueInformer.Services;

public class RabbitMQService : IAsyncDisposable
{
    private readonly ILogger<RabbitMQService> _logger;
    private readonly ConnectionFactory _connectionFactory;
    private readonly RedisService _redisService;
    private readonly IConfiguration _configuration;

    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string[] _queueNames;

    public event EventHandler<string>? MessageReceived;

    public RabbitMQService(ILogger<RabbitMQService> logger, IConfiguration configuration, RedisService redisService)
    {
        _logger = logger;
        _configuration = configuration;
        _redisService = redisService;
        var rabbitConfig = configuration.GetSection("RabbitMQ");

        _connectionFactory = new ConnectionFactory()
        {
          HostName = rabbitConfig["HostName"] ?? "localhost",
          Port = int.Parse(rabbitConfig["Port"] ?? "5672"),
          UserName = rabbitConfig["UserName"] ?? "guest",
          Password = rabbitConfig["Password"] ?? "guest",
          VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
        };
        _queueNames = new[] { configuration["Queues:WindowsQueue"] ?? "WindowsQueue", configuration["Queues:TalonQueue"] ?? "TalonQueue", configuration["Queues:Queue"] ?? "Queue" };
  }

  public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
  {
    _connection = await _connectionFactory.CreateConnectionAsync();
    _channel = await _connection.CreateChannelAsync();

        // Объявляем все очереди
        foreach (var queueName in _queueNames)
        {
            await _channel.ExchangeDeclareAsync(
                exchange: queueName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false
            );

            await _channel.QueueDeclareAsync(
                queue: $"{queueName}-QueueInformer",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken
            );

            await _channel.QueueBindAsync(
                queue: $"{queueName}-QueueInformer",
                exchange: queueName,
                routingKey: ""
            );
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);


    consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<BaseDTO>(messageJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DTOJsonConverter(), new JsonStringEnumConverter() }
                });

                var jsonMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions
                {
                    Converters = { new DTOJsonConverter() }
                });

                if (message != null)
                {
                    MessageReceived?.Invoke(this, jsonMessage);
                    await _redisService.AddMessage(message, jsonMessage);

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

        foreach (var queueName in _queueNames)
        {
            await _channel.BasicConsumeAsync(
                queue: $"{queueName}-QueueInformer",
                autoAck: false, // важно: ручное подтверждение
                consumer: consumer,
                cancellationToken: cancellationToken
            );

            Console.WriteLine($"Subscribed to queue: {queueName}-QueueInformer");
        }
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