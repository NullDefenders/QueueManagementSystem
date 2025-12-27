using QueueService.Factory;
using StackExchange.Redis;

namespace QueueService.Services;

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
  private async Task<RedisResult> AddIfNotExistsAsync(String Key, String Id)
  {
    string lua = @"-- Скрипт для атомарного добавления значения в список, 
      -- только если он там еще не присутствует.

      -- KEYS[1]: Ключ списка (например, 'TALONS')
      -- ARGV[1]: id - Идентификатор который нужно добавить.

      local KEY = KEYS[1]
      local ID = ARGV[1]

      -- 1. Проверяем, существует ли элемент в списке.
      local list_elements = redis.call('LRANGE', KEY, 0, -1)

      local exists = 0
      for i, element in ipairs(list_elements) do
          if element == ID then
              exists = 1
              break
          end
      end

      if exists > 0 then          -- Элемент уже существует. Ничего не делаем.
          return 0 -- Возвращаем 0 (дубликат)
      else
          -- Элемент не существует. Добавляем его в конец списка (FIFO).
          redis.call('RPUSH', KEY, ID)
          return 1 -- Возвращаем 1 (успешно добавлен)
      end
    ";
    return await _redisdb.ScriptEvaluateAsync(
        lua,
        new RedisKey[] { Key },
        new RedisValue[] { Id }
    );
  }
  public async Task AddTalon(string TalonNumber, double? PendingTime)
  {
    if (PendingTime != null)
    {
      await _redisdb.SortedSetAddAsync("PENDING", TalonNumber, (double)PendingTime);
    }
    await AddIfNotExistsAsync("TALONS", TalonNumber);
  }

  public async Task<RedisResult[]> GetTalon()
  {
    string lua = @"
      -- Скрипт для Redis на Lua, реализующий логику приоритетов для двух очередей.
      -- PENDING (Sorted Set) и TALONS (List).

      -- Ключи, передаваемые в скрипт (KEYS):
      -- KEYS[1]: Ключ списка TALONS (например, 'TALONS')
      -- KEYS[2]: Ключ сортированного множества PENDING (например, 'PENDING')
      -- KEYS[3]: Ключ для счетчика последовательных талонов PENDING (например, 'counter')

      -- Аргументы, передаваемые в скрипт (ARGV):
      -- ARGV[1]: max_pending_served (int) - Максимальное количество талонов PENDING, которое можно вызвать подряд.
      -- ARGV[2]: current_time_seconds (float/int) - Текущее время в секундах с начала дня.
      -- ARGV[3]: pending_lookahead_seconds (int) - Окно просмотра вперед (в секундах) от текущего времени.

      local TALONS_KEY = KEYS[1]
      local PENDING_KEY = KEYS[2]
      local COUNT_KEY = KEYS[3]

      local MAX_PENDING = tonumber(ARGV[1])
      local CURRENT_TIME = tonumber(ARGV[2])
      local LOOKAHEAD = tonumber(ARGV[3])

      -- Коды статуса для возврата клиенту
      local STATUS_SERVED_PENDING = 1
      local STATUS_SERVED_TALONS = 2
      local STATUS_NOTHING_AVAILABLE = 0

      -- 1. Получаем текущее значение счетчика PENDING
      local pending_count = redis.call('GET', COUNT_KEY)
      if pending_count then
          pending_count = tonumber(pending_count)
      else
          pending_count = 0
      end

      -- 2. Определяем максимально допустимый балл для талона PENDING
      -- Талон считается ""готовым"" к вызову, если его запланированное время (балл)
      -- находится в пределах [0, CURRENT_TIME + LOOKAHEAD].
      local max_eligible_score = CURRENT_TIME + LOOKAHEAD
      local min_eligible_score = 0

      -- 3. Проверяем наличие подходящего талона PENDING
      -- Выбираем талон с наименьшим баллом (самое раннее время) в пределах окна.
      local pending_item = redis.call('ZRANGEBYSCORE', PENDING_KEY, min_eligible_score, max_eligible_score, 'WITHSCORES', 'LIMIT', 0, 1)

      local has_pending = #pending_item > 0
      local has_talons = redis.call('LLEN', TALONS_KEY) > 0

      local served_item = nil
      local served_queue_status = STATUS_NOTHING_AVAILABLE

      -- -- -- ЛОГИКА ПРИОРИТЕТОВ -- -- --

      -- Приоритет 1: PENDING, если он доступен И счетчик не превышен
      if has_pending and pending_count < MAX_PENDING then
          -- Обслуживаем PENDING
          local item_id = pending_item[1]
    
          -- 1a. Удаляем талон из PENDING Sorted Set
          redis.call('ZREM', PENDING_KEY, item_id)
    
          -- 1b. Удаляем талон из TALONS List (удаляем только первое совпадение)
          redis.call('LREM', TALONS_KEY, 1, item_id)
    
          -- Увеличиваем счетчик PENDING
          pending_count = pending_count + 1
          redis.call('SET', COUNT_KEY, pending_count)

          served_item = item_id
          served_queue_status = STATUS_SERVED_PENDING

      -- Приоритет 2: TALONS, если он доступен (или если лимит PENDING превышен)
      elseif has_talons then
          -- Обслуживаем TALONS
          local item_id = redis.call('LPOP', TALONS_KEY)
    
          if item_id then
              -- Удаляем элемент из PENDING Sorted Set на случай, если он там был (талон предв. записи)
              redis.call('ZREM', PENDING_KEY, item_id)
    
              -- Сбрасываем счетчик PENDING, так как мы обслужили талон из обычной очереди
              redis.call('SET', COUNT_KEY, 0)
    
              served_item = item_id
              served_queue_status = STATUS_SERVED_TALONS
          end
      end

      -- Возвращаем результат
      if served_item then
          local queue_name = (served_queue_status == STATUS_SERVED_PENDING) and ""PENDING"" or ""TALONS""
          -- Возвращаем {ID талона, код статуса, название очереди}
          return {served_item, served_queue_status, queue_name}
      else
          -- Ничего не найдено
          return {nil, STATUS_NOTHING_AVAILABLE, ""NONE""}
      end
      ";
    return (RedisResult[]?)await _redisdb.ScriptEvaluateAsync(
        lua,
        new RedisKey[] { "TALONS", "PENDING", "counter" },
        new RedisValue[] { _configuration.GetValue<int>("Settings:PendingCount"), DateTime.Now.TimeOfDay.TotalSeconds, _configuration.GetValue<int>("Settings:MinutesBeforePending") * 60 }
    ) ?? [];
  }

  public async Task AddWindow(string WindowNumber)
  {
    await AddIfNotExistsAsync("WINDOWS", WindowNumber);
  }

  public async Task<RedisValue> GetWindow()
  {
    return await _redisdb.ListRightPopAsync("WINDOWS");
  }

  public async Task DelWindow(string WindowNumber)
  {
    await _redisdb.ListRemoveAsync("WINDOWS", WindowNumber);
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
