using Newtonsoft.Json;
using QueueService.DTO;
using QueueService.Factory;
using QueueService.Helper;
using QueueService.Services;

namespace QueueService.Domain;

public class TalonMessageProcessor : IMessageProcessor
{
  private readonly ILogger<TalonMessageProcessor> _logger;
  private readonly IConfiguration _configuration;
  private readonly IQueueStorage _storage;
  private readonly RabbitMQService _rabbitMQService;

  public TalonMessageProcessor(ILogger<TalonMessageProcessor> logger, IConfiguration configuration, IQueueStorage storage, RabbitMQService rabbitMQService)
  {
    _logger = logger;
    _configuration = configuration;
    _storage = storage;
    _rabbitMQService = rabbitMQService;
  }

  public async Task ProcessAsync(string message)
  {
    _logger.LogInformation($"Talon processing: {message}");

    var settings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore
    };
    var talons = new List<TalonDTO>();

    if (message.TrimStart().StartsWith('['))
    {
      talons = JsonConvert.DeserializeObject<List<TalonDTO>>(message, settings);
    }
    else if (message.TrimStart().StartsWith('{'))
    {
      talons = [JsonConvert.DeserializeObject<TalonDTO>(message, settings)];
    }

    foreach (var talon in talons)
    {
      if (talon != null && !string.IsNullOrEmpty(talon.TalonNumber) && !string.IsNullOrEmpty(talon.ServiceCode))
      {
        var window = await _storage.GetWindowAsync();
        if (string.IsNullOrEmpty(window))
        {
          int minutesLimit = _configuration.GetValue<int>("Settings:MinutesAfterPending");
          var cutoffTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(-minutesLimit));
          double? pendingTimeSeconds = null;
          if (talon.PendingTime != null)
          {
            pendingTimeSeconds = talon.PendingTime?.TotalSeconds ?? 0;
            if (pendingTimeSeconds < cutoffTime.TotalSeconds)
            {
              pendingTimeSeconds = null;
            }
          }
          await _storage.AddTalonAsync(talon.TalonNumber, pendingTimeSeconds);

        }
        else
        {
          var queue = new QueueDTO { TalonNumber = talon.TalonNumber, WindowNumber = window };
          await _rabbitMQService.SendQueue(queue);
        }

        _logger.LogInformation($"Talon processed: {message}");
      }
      else
        _logger.LogError($"Talon message is not correct: {message}");
    }
  }
}