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
  public async Task<RedisResult> AddIfNotExistsAsync(String Key, String Id) {
    string lua = @"-- ������ ��� ���������� ���������� �������� � ������, 
      -- ������ ���� �� ��� ��� �� ������������.

      -- KEYS[1]: ���� ������ (��������, 'TALONS')
      -- ARGV[1]: id - ������������� ������� ����� ��������.

      local KEY = KEYS[1]
      local ID = ARGV[1]

      -- 1. ���������, ���������� �� ������� � ������.
      local list_elements = redis.call('LRANGE', KEY, 0, -1)

      local exists = 0
      for i, element in ipairs(list_elements) do
          if element == ID then
              exists = 1
              break
          end
      end

      if exists > 0 then          -- ������� ��� ����������. ������ �� ������.
          return 0 -- ���������� 0 (��������)
      else
          -- ������� �� ����������. ��������� ��� � ����� ������ (FIFO).
          redis.call('RPUSH', KEY, ID)
          return 1 -- ���������� 1 (������� ��������)
      end
    ";
    return await _redisdb.ScriptEvaluateAsync(
        lua,
        new RedisKey[] { Key },
        new RedisValue[] { Id }
    );
  }

  public async Task<RedisResult[]> GetTalon()
  {
    string lua = @"
      -- ������ ��� Redis �� Lua, ����������� ������ ����������� ��� ���� ��������.
      -- PENDING (Sorted Set) � TALONS (List).

      -- �����, ������������ � ������ (KEYS):
      -- KEYS[1]: ���� ������ TALONS (��������, 'TALONS')
      -- KEYS[2]: ���� �������������� ��������� PENDING (��������, 'PENDING')
      -- KEYS[3]: ���� ��� �������� ���������������� ������� PENDING (��������, 'counter')

      -- ���������, ������������ � ������ (ARGV):
      -- ARGV[1]: max_pending_served (int) - ������������ ���������� ������� PENDING, ������� ����� ������� ������.
      -- ARGV[2]: current_time_seconds (float/int) - ������� ����� � �������� � ������ ���.
      -- ARGV[3]: pending_lookahead_seconds (int) - ���� ��������� ������ (� ��������) �� �������� �������.

      local TALONS_KEY = KEYS[1]
      local PENDING_KEY = KEYS[2]
      local COUNT_KEY = KEYS[3]

      local MAX_PENDING = tonumber(ARGV[1])
      local CURRENT_TIME = tonumber(ARGV[2])
      local LOOKAHEAD = tonumber(ARGV[3])

      -- ���� ������� ��� �������� �������
      local STATUS_SERVED_PENDING = 1
      local STATUS_SERVED_TALONS = 2
      local STATUS_NOTHING_AVAILABLE = 0

      -- 1. �������� ������� �������� �������� PENDING
      local pending_count = redis.call('GET', COUNT_KEY)
      if pending_count then
          pending_count = tonumber(pending_count)
      else
          pending_count = 0
      end

      -- 2. ���������� ����������� ���������� ���� ��� ������ PENDING
      -- ����� ��������� ""�������"" � ������, ���� ��� ��������������� ����� (����)
      -- ��������� � �������� [0, CURRENT_TIME + LOOKAHEAD].
      local max_eligible_score = CURRENT_TIME + LOOKAHEAD
      local min_eligible_score = 0

      -- 3. ��������� ������� ����������� ������ PENDING
      -- �������� ����� � ���������� ������ (����� ������ �����) � �������� ����.
      local pending_item = redis.call('ZRANGEBYSCORE', PENDING_KEY, min_eligible_score, max_eligible_score, 'WITHSCORES', 'LIMIT', 0, 1)

      local has_pending = #pending_item > 0
      local has_talons = redis.call('LLEN', TALONS_KEY) > 0

      local served_item = nil
      local served_queue_status = STATUS_NOTHING_AVAILABLE

      -- -- -- ������ ����������� -- -- --

      -- ��������� 1: PENDING, ���� �� �������� � ������� �� ��������
      if has_pending and pending_count < MAX_PENDING then
          -- ����������� PENDING
          local item_id = pending_item[1]
    
          -- 1a. ������� ����� �� PENDING Sorted Set
          redis.call('ZREM', PENDING_KEY, item_id)
    
          -- 1b. ������� ����� �� TALONS List (������� ������ ������ ����������)
          redis.call('LREM', TALONS_KEY, 1, item_id)
    
          -- ����������� ������� PENDING
          pending_count = pending_count + 1
          redis.call('SET', COUNT_KEY, pending_count)

          served_item = item_id
          served_queue_status = STATUS_SERVED_PENDING

      -- ��������� 2: TALONS, ���� �� �������� (��� ���� ����� PENDING ��������)
      elseif has_talons then
          -- ����������� TALONS
          local item_id = redis.call('LPOP', TALONS_KEY)
    
          if item_id then
              -- ������� ������� �� PENDING Sorted Set �� ������, ���� �� ��� ��� (����� �����. ������)
              redis.call('ZREM', PENDING_KEY, item_id)
    
              -- ���������� ������� PENDING, ��� ��� �� ��������� ����� �� ������� �������
              redis.call('SET', COUNT_KEY, 0)
    
              served_item = item_id
              served_queue_status = STATUS_SERVED_TALONS
          end
      end

      -- ���������� ���������
      if served_item then
          local queue_name = (served_queue_status == STATUS_SERVED_PENDING) and ""PENDING"" or ""TALONS""
          -- ���������� {ID ������, ��� �������, �������� �������}
          return {served_item, served_queue_status, queue_name}
      else
          -- ������ �� �������
          return {nil, STATUS_NOTHING_AVAILABLE, ""NONE""}
      end
      ";
    return (RedisResult[]?)await _redisdb.ScriptEvaluateAsync(
        lua,
        new RedisKey[] { "TALONS", "PENDING", "counter" },
        new RedisValue[] { _configuration.GetValue<int>("Settings:PendingCount"), DateTime.Now.TimeOfDay.TotalSeconds, _configuration.GetValue<int>("Settings:MinutesBeforePending") * 60 }
    ) ?? [];
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
