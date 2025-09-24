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
    // Ѕизнес-логика дл€ WindowsQueue
    var settings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore
    };

    var window = JsonConvert.DeserializeObject<WindowDTO>(message, settings);

    if (window != null && !string.IsNullOrEmpty(window.WindowNumber) && window.Status != null)
    {
      if (window.Status == WindowsStatus.free)
      {
        string lua = @"
-- KEYS:
-- 1 = talons list key (LIST)
-- 2 = pending zset key (ZSET)
-- 3 = counter key (STRING)
--
-- ARGV:
-- 1 = maxPre (integer)
-- 2 = nowSeconds (integer)
-- 3 = allowedBeforeSeconds (integer) -- minutesBefore * 60

local talonsKey = KEYS[1]
local pendingKey = KEYS[2]
local counterKey = KEYS[3]

local maxPre = tonumber(ARGV[1])
local now = tonumber(ARGV[2])
local allowedBefore = tonumber(ARGV[3])

local function get_first_in_range(mins, maxs)
  local res = redis.call('ZRANGEBYSCORE', pendingKey, tostring(mins), tostring(maxs), 'LIMIT', 0, 1)
  if res[1] then return res[1] end
  return nil
end

local function find_pending_candidate()
  local threshold = now + allowedBefore
  return get_first_in_range(0, threshold)
end

-- счЄтчик сколько подр€д вз€ли из PENDING
local preTaken = tonumber(redis.call('GET', counterKey) or '0')

-- если ещЄ не достигли maxPre Ч пробуем вз€ть из PENDING
if preTaken < maxPre then
  local cand = find_pending_candidate()
  if cand then
    redis.call('ZREM', pendingKey, cand)
    redis.call('LREM', talonsKey, 0, cand)
    redis.call('SET', counterKey, preTaken + 1)
    return { cand, ""pending"" }
  end
end

-- иначе берЄм из TALONS
local len = tonumber(redis.call('LLEN', talonsKey) or '0')
for i = 1, len do
  local t = redis.call('LPOP', talonsKey)
  if not t then break end
  if redis.call('ZSCORE', pendingKey, t) then
    -- если талон есть в pending Ч ротаци€ в конец
    redis.call('RPUSH', talonsKey, t)
  else
    redis.call('SET', counterKey, 0)
    return { t, ""normal"" }
  end
end

return nil
";

        var result = await _redisService._redisdb.ScriptEvaluateAsync(
            lua,
            new RedisKey[] { "TALONS", "PENDING", "counter" },
            new RedisValue[] { _configuration.GetValue<int>("Settings:PendingCount"), DateTime.Now.TimeOfDay.TotalSeconds, _configuration.GetValue<int>("Settings:MinutesBeforePending") }
        );

        String talon = "";

        if (!result.IsNull)
        {
          var arr = (RedisResult[])result;

          talon = arr[0].ToString();
        }

        if (String.IsNullOrEmpty(talon))
        {
          await _redisService._redisdb.ListLeftPushAsync("WINDOWS", window.WindowNumber);
        }
        else
        {
//          await _redisService._redisdb.SortedSetRemoveAsync("PENDING", talon);
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