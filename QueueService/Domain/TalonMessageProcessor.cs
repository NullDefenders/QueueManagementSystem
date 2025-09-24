using Newtonsoft.Json;
using QueueService.DTO;
using QueueService.Factory;
using QueueService.Services;

namespace QueueService.Domain;

public class TalonMessageProcessor : IMessageProcessor
{
  private readonly ILogger<TalonMessageProcessor> _logger;
  private readonly IConfiguration _configuration;
  private readonly RedisService _redisService;
  private readonly RabbitMQService _rabbitMQService;

  public TalonMessageProcessor(ILogger<TalonMessageProcessor> logger, IConfiguration configuration, RedisService redisService, RabbitMQService rabbitMQService)
  {
    _logger = logger;
    _configuration = configuration;
    _redisService = redisService;
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

    var talon = JsonConvert.DeserializeObject<TalonDTO>(message, settings);

    if (talon != null && !string.IsNullOrEmpty(talon.TalonNumber) && !string.IsNullOrEmpty(talon.ServiceCode))
    {
      var window = await _redisService._redisdb.ListLeftPopAsync ("WINDOWS");
      if (window.IsNullOrEmpty) {
        if (talon.PendingTime != null)
        {
          int MinutesAfterPending = _configuration.GetValue<int>("Settings:MinutesAfterPending");
          double score = talon.PendingTime?.TotalSeconds ?? 0;
          if (score >= (DateTime.Now.TimeOfDay.TotalSeconds - MinutesAfterPending * 60))
            await _redisService._redisdb.SortedSetAddAsync("PENDING", talon.TalonNumber, score);
        }
//        await _redisService._redisdb.ListLeftPushAsync($"TALONS:{talon.ServiceCode}", talon.TalonNumber);
        await _redisService._redisdb.ListLeftPushAsync("TALONS", talon.TalonNumber);
      }
      else
      {
        await _redisService._redisdb.SortedSetRemoveAsync("PENDING", talon.TalonNumber);
        var queue = new QueueDTO { TalonNumber = talon.TalonNumber, WindowNumber = window};
        await _rabbitMQService.SendQueue(queue);
      }

      _logger.LogInformation($"Talon processed: {message}");
    }
    else
      _logger.LogError($"Talon message is not correct: {message}");
  }
}