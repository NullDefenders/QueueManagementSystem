using QueueService.Domain;
using QueueService.Services;

namespace QueueService.Factory;
public class MessageProcessorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MessageProcessorFactory> _logger;


  public MessageProcessorFactory(ILogger<MessageProcessorFactory> logger, IServiceProvider serviceProvider)
    {
      _logger = logger;
      _serviceProvider = serviceProvider;
    }

    public IMessageProcessor CreateProcessor(string queueName)
    {
        return queueName switch
        {
            "TalonQueue" => _serviceProvider.GetRequiredService<TalonMessageProcessor>(),
            "WindowsQueue" => _serviceProvider.GetRequiredService<WindowMessageProcessor>(),
            _ => throw new ArgumentException($"Unknown queue: {queueName}")
        };
    }
}