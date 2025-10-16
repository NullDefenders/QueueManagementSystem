using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QueueService.DTO;
using QueueService.Services;
using StackExchange.Redis;

namespace QueueService.Domain;

public class WindowMessageProcessor : IMessageProcessor
{
  private readonly ILogger<WindowMessageProcessor> _logger;
  private readonly IConfiguration _configuration;
  private readonly RedisService _redisService;
  private readonly RabbitMQService _rabbitMQService;

  public WindowMessageProcessor(ILogger<WindowMessageProcessor> logger, IConfiguration configuration, RedisService redisService, RabbitMQService rabbitMQService)
  {
    _logger = logger;
    _configuration = configuration;
    _redisService = redisService;
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
          var result = await _redisService.GetTalon();

          String talon = "";

          if (result.Length > 0)
          {
            talon = result[0].ToString();
          }

          if (String.IsNullOrEmpty(talon))
          {
            await _redisService.AddIfNotExistsAsync("WINDOWS", window.WindowNumber);
          }
          else
          {
            var queue = new QueueDTO { TalonNumber = talon, WindowNumber = window.WindowNumber };
            await _rabbitMQService.SendQueue(queue);
          }
        }
        else
        {
          await _redisService._redisdb.ListRemoveAsync("WINDOWS", window.WindowNumber);
        }
        _logger.LogInformation($"Windows processed: {message}");
      }
      else
        _logger.LogError($"Windows message is not correct: {message}");
    }
  }
}