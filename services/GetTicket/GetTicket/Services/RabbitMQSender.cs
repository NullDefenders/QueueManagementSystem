using GetTicket.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GetTicket.Services
{
    public interface ISimpleRabbitMQSender
    {
        ValueTask DisposeAsync();
        Task SendTicketAsync(Ticket ticket);
    }

    public class SimpleRabbitMQSender : ISimpleRabbitMQSender, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ILogger<SimpleRabbitMQSender> _logger;

        public SimpleRabbitMQSender(ILogger<SimpleRabbitMQSender> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "password",
                VirtualHost = "/"
            };

            _connection = Task.Run(async () => await factory.CreateConnectionAsync()).GetAwaiter().GetResult();
            _channel = Task.Run(async () => await _connection.CreateChannelAsync()).GetAwaiter().GetResult();

            _channel.QueueDeclareAsync(
                queue: "TalonQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public async Task SendTicketAsync(Ticket ticket)
        {
            try
            {
                var message = new
                {
                    MessageId = Guid.NewGuid().ToString(),
                    TicketId = ticket.Id.ToString(),
                    ticket.TalonNumber,
                    Action = "TicketCreated",
                    Timestamp = ticket.IssuedAt,
                    ticket.ServiceCode,
                    ticket.PendingTime
                };

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = new BasicProperties()
                {
                    Persistent = true,
                    MessageId = Guid.NewGuid().ToString(),
                    ContentType = "application/json",
                    ContentEncoding = "utf-8"
                };

                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "TalonQueue",
                    mandatory: false,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation("✅ Ticket {TicketNumber} sent directly to RabbitMQ", ticket.TalonNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error sending ticket {TicketNumber} to RabbitMQ", ticket.TalonNumber);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }
        }
    }
}