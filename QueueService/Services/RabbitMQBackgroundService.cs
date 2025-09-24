using QueueService.Services;

public class RabbitMQBackgroundService : BackgroundService
{
    private readonly RabbitMQService _rabbitMqService;
    private readonly ILogger<RabbitMQBackgroundService> _logger;

    public RabbitMQBackgroundService(
        RabbitMQService rabbitMqService, 
        ILogger<RabbitMQBackgroundService> logger)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting RabbitMQ consumers...");

        try
        {
            await _rabbitMqService.StartConsumingAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting RabbitMQ consumers");
        }

        // Ждем пока не будет запрошена остановка
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation("Stopping RabbitMQ consumers...");
    }
}