using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QueueService.DTO;
using QueueService.Helper;
using QueueService.Services;
using StackExchange.Redis;

namespace QueueService.Domain;

public class WindowMessageProcessor : IMessageProcessor
{
  private readonly ILogger<WindowMessageProcessor> _logger;
  private readonly IConfiguration _configuration;
  private readonly IQueueStorage _storage;
  private readonly RabbitMQService _rabbitMQService;

  public WindowMessageProcessor(ILogger<WindowMessageProcessor> logger, IConfiguration configuration, IQueueStorage storage, RabbitMQService rabbitMQService)
  {
    _logger = logger;
    _configuration = configuration;
    _storage = storage;
    _rabbitMQService = rabbitMQService;
  }

  public async Task ProcessAsync(string message)
  {
    _logger.LogInformation($"Windows processing: {message}");
    // Бизнес-логика для WindowsQueue
    var settings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore
    };

    var windows = new List<WindowDTO>();

    if (message.TrimStart().StartsWith('['))
    {
      windows = JsonConvert.DeserializeObject<List<WindowDTO>>(message, settings);
    }
    else if (message.TrimStart().StartsWith('{'))
    {
      windows = [JsonConvert.DeserializeObject<WindowDTO>(message, settings)];
    }

    foreach (var window in windows)
    {
      if (window != null && !string.IsNullOrEmpty(window.WindowNumber) && window.Status != null)
      {
        if (window.Status == WindowsStatus.free)
        {
          var talon = await _storage.GetNextTalonAsync();

          if (String.IsNullOrEmpty(talon))
          {
            await _storage.AddWindowAsync(window.WindowNumber);
          }
          else
          {
            var queue = new QueueDTO { TalonNumber = talon, WindowNumber = window.WindowNumber };
            await _rabbitMQService.SendQueue(queue);
          }
        }
        else
        {
          await _storage.RemoveWindowAsync(window.WindowNumber);
        }
        _logger.LogInformation($"Windows processed: {message}");
      }
      else
        _logger.LogError($"Windows message is not correct: {message}");
    }
  }
}