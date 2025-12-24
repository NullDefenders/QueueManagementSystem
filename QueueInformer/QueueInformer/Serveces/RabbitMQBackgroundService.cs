namespace QueueInformer.Services;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly RabbitMQService _rabbitMQService;
    private readonly ISseService _sseService;
    private readonly ILogger<RabbitMQBackgroundService> _logger;

    public RabbitMQBackgroundService(
        RabbitMQService rabbitMQService,
        ISseService sseService,
        ILogger<RabbitMQBackgroundService> logger)
    {
        _rabbitMQService = rabbitMQService;
        _sseService = sseService;
        _logger = logger;
    }

    protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQService.MessageReceived += OnMessageReceived;
        await _rabbitMQService.StartConsumingAsync();
        
        _logger.LogInformation("RabbitMQ background service started");
        
        return Task.CompletedTask;
    }

    private void OnMessageReceived(object? sender, string jsonMessage)
    {
        try
        {
            
            _sseService.SendToAll(jsonMessage);
            _logger.LogInformation("Message sent via SSE: {MessageId}", jsonMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message via SSE");
        }
    }
}