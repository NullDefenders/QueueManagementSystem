using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace WindowService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WindowController : ControllerBase
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ILogger<WindowController> _logger;

        public WindowController(ILogger<WindowController> logger)
        {
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672,
                UserName = "admin",
                Password = "password",
                VirtualHost = "/"
            };

            _connection = Task.Run(async () => await factory.CreateConnectionAsync()).GetAwaiter().GetResult();
            _channel = Task.Run(async () => await _connection.CreateChannelAsync()).GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(
                exchange: "WindowsQueue",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                arguments: null
            );
        }

        [HttpPost]
        public async Task SendWindowStatus(WindowDTO windowDto)
        {
            try
            {
                string status = windowDto.Status == WindowsStatus.free ? "free" : "busy";
                Console.WriteLine(status);
                var message = new
                {
                    windowDto.WindowNumber,
                    status,
                };

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties =  new BasicProperties()
                {
                    Persistent = true,
                    MessageId = Guid.NewGuid().ToString(),
                    ContentType = "application/json",
                    ContentEncoding = "utf-8"
                };

                await _channel.BasicPublishAsync(
                    exchange: "WindowsQueue",
                    routingKey: "",
                    mandatory: false,
                    basicProperties: properties,
                    body: body
                );

                _logger.LogInformation("✅ Window status sent to RabbitMQ: {WindowNumber} - {Status}",
                    windowDto.WindowNumber, windowDto.Status);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error sending window status to RabbitMQ", windowDto.WindowNumber, windowDto.Status);
            }
        }

        public enum WindowsStatus { free, busy };

        public class WindowDTO
        {
            public string? WindowNumber { get; set; }
            public WindowsStatus Status { get; set; }
        }
    }
}