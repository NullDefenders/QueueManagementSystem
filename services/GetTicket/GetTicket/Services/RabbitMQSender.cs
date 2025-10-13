using GetTicket.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GetTicket.Services
{
    public interface ISimpleRabbitMQSender
    {
        Task SendTicketAsync(Ticket ticket);
    }

    public class SimpleRabbitMQSender : ISimpleRabbitMQSender, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ILogger<SimpleRabbitMQSender> _logger;

        public SimpleRabbitMQSender(IConfiguration configuration, ILogger<SimpleRabbitMQSender> logger)
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
                queue: "talon-queue",
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
                    TicketNumber = ticket.TicketNumber,
                    Action = "TicketCreated",
                    Timestamp = ticket.IssuedAt,
                    Data = new
                    {
                        ServiceName = ticket.ServiceName,
                        IssuedAt = ticket.IssuedAt
                    }
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
                    routingKey: "talon-queue",
                    mandatory: false,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation("✅ Ticket {TicketNumber} sent directly to RabbitMQ", ticket.TicketNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error sending ticket {TicketNumber} to RabbitMQ", ticket.TicketNumber);
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