using QueueInformer.Models;
using StackExchange.Redis;

namespace QueueInformer.Serveces;

public class RedisService : IAsyncDisposable
{
  private readonly ILogger<RedisService> _logger;
  private readonly IConfiguration _configuration;
  private readonly IConnectionMultiplexer _redisConnection;
  public IDatabase _redisdb;

  public RedisService(ILogger<RedisService> logger, IConfiguration configuration)
  {
    _logger = logger;
    _configuration = configuration;
    var redisConfig = configuration.GetSection("Redis");
    var options = new ConfigurationOptions
    {
      EndPoints = { { redisConfig["Host"] ?? "localhost", int.Parse(redisConfig["Port"] ?? "6379") } },
      DefaultDatabase = int.Parse(redisConfig["Database"]??"1"),
      AllowAdmin = bool.Parse(redisConfig["AllowAdmin"] ?? "false"),
      ConnectRetry = int.Parse(redisConfig["ConnectRetry"] ?? "3"),
      ConnectTimeout = int.Parse(redisConfig["ConnectTimeout"] ?? "5000"),
      SyncTimeout = int.Parse(redisConfig["SyncTimeout"] ?? "5000")
    };

    _redisConnection = ConnectionMultiplexer.Connect(options);
    _redisdb = _redisConnection.GetDatabase();
  }
  public async Task AddMessage(BaseDTO message, string jsonMessage) {
    switch (message)
    {
      case TalonDTO talon:
        // работаем с талоном
        await _redisdb.HashSetAsync("TalonQueue", talon.TalonNumber, jsonMessage);
        break;
      case WindowDTO window:
        // работаем с окном
        await _redisdb.HashDeleteAsync("Queue", window.WindowNumber);
        await _redisdb.HashSetAsync("WindowsQueue", window.WindowNumber, jsonMessage);
        break;
      case QueueDTO queue:
        await _redisdb.HashDeleteAsync("TalonQueue", queue.TalonNumber);
        await _redisdb.HashDeleteAsync("WindowsQueue", queue.WindowNumber);
        await _redisdb.HashSetAsync("Queue", queue.WindowNumber, jsonMessage);
        // работаем с очередью
        break;
    }
  }

    public async Task<List<String>> GetMessages()
  {
        var TalonQueueList = (await _redisdb.HashGetAllAsync("TalonQueue")).Select(x => x.Value.ToString()).ToList();
        var WindowsQueueList = (await _redisdb.HashGetAllAsync("WindowsQueue")).Select(x => x.Value.ToString()).ToList();
        var QueueList = (await _redisdb.HashGetAllAsync("Queue")).Select(x => x.Value.ToString()).ToList();
        return TalonQueueList
          .Concat(WindowsQueueList)
          .Concat(QueueList)
          .ToList();
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
