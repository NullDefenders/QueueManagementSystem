using QueueService.Factory;
using StackExchange.Redis;

namespace QueueService.Services;

public interface IRedisService
{
  Task<T> GetAsync<T>(string key);
  Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
  Task<bool> RemoveAsync(string key);
  Task<bool> ExistsAsync(string key);
  Task<TimeSpan?> GetTimeToLiveAsync(string key);
}

//public class RedisService : IRedisService, IAsyncDisposable
public class RedisService : IAsyncDisposable
{
  private readonly ILogger<RedisService> _logger;
  private readonly IConfiguration _configuration;
  private readonly IConnectionMultiplexer _redisConnection;
  public IDatabase _redisdb;

  public RedisService(ILogger<RedisService> logger, IConfiguration configuration, MessageProcessorFactory processorFactory)
  {
    _logger = logger;
    _configuration = configuration;
    var redisConfig = configuration.GetSection("Redis");
    var options = new ConfigurationOptions
    {
      EndPoints = { { redisConfig["Host"] ?? "localhost", int.Parse(redisConfig["Port"] ?? "6379") } },
      AllowAdmin = bool.Parse(redisConfig["AllowAdmin"] ?? "false"),
      ConnectRetry = int.Parse(redisConfig["ConnectRetry"] ?? "3"),
      ConnectTimeout = int.Parse(redisConfig["ConnectTimeout"] ?? "5000"),
      SyncTimeout = int.Parse(redisConfig["SyncTimeout"] ?? "5000")
    };

    _redisConnection = ConnectionMultiplexer.Connect(options);
    _redisdb = _redisConnection.GetDatabase();
  }

  public async ValueTask DisposeAsync()
  {
    if (_redisConnection != null)
    {
      await _redisConnection.CloseAsync();
      _redisConnection.Dispose();
    }
  }
}
