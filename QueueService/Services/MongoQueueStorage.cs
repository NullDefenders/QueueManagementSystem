using MongoDB.Bson;
using MongoDB.Driver;
using QueueService.DTO;
using QueueService.Helper;

public class MongoQueueStorage : IQueueStorage
{
  private readonly IMongoCollection<TalonDocument> _talons;
  private readonly IMongoCollection<WindowDocument> _windows;
  private readonly IMongoCollection<QueueState> _queueState;
  private readonly IMongoClient _mongoClient;
  private readonly IConfiguration _configuration;

  public MongoQueueStorage(IConfiguration configuration)
  {
    _configuration = configuration;

    var mongoSettings = configuration.GetSection("Mongo");
    var connectionString = mongoSettings.GetValue<string>("ConnectionString") ?? "mongodb://localhost:27017";
    _mongoClient = new MongoClient(connectionString);

    var db = _mongoClient.GetDatabase(mongoSettings.GetValue<string>("Database") ?? "QueueDb");

    _talons = db.GetCollection<TalonDocument>("talons");
    _windows = db.GetCollection<WindowDocument>("windows");
    _queueState = db.GetCollection<QueueState>("queue_state");
  }

  public async Task InitializeAsync()
  {
    await _talons.Indexes.CreateManyAsync([
      new CreateIndexModel<TalonDocument>(
        Builders<TalonDocument>.IndexKeys
          .Ascending(x => x.PendingTime)
      ),
      new CreateIndexModel<TalonDocument>(
        Builders<TalonDocument>.IndexKeys
          .Ascending(x => x.CreatedAt)
      )
    ]);

    await _queueState.Indexes.CreateManyAsync([
      new CreateIndexModel<QueueState>(
        Builders<QueueState>.IndexKeys
          .Ascending(x => x.Id)
      )
    ]);

    await _windows.Indexes.CreateManyAsync(
    [
      new CreateIndexModel<WindowDocument>(
        Builders<WindowDocument>.IndexKeys
          .Ascending(x => x.CreatedAt)
      )
    ]);

    await _queueState.UpdateOneAsync(
      x => x.Id == "main",
      Builders<QueueState>.Update.SetOnInsert(x => x.PendingServedCount, 0),
      new UpdateOptions { IsUpsert = true }
    );
  }

  public async Task AddTalonAsync(string talonNumber, double? pendingTime)
  {
    await _talons.InsertOneAsync(new TalonDocument
    {
      Number = talonNumber,
      CreatedAt = DateTime.UtcNow,
      PendingTime = pendingTime
    });
  }

  public async Task<string?> GetNextTalonAsync()
  {
    var threshold =
        DateTime.Now.TimeOfDay.TotalSeconds +
        _configuration.GetValue<int>("Settings:MinutesBeforePending") * 60;

    var maxPending = _configuration.GetValue<int>("Settings:PendingCount");

    using var session = await _mongoClient.StartSessionAsync();
    session.StartTransaction();

    try
    {
      var state = await _queueState
          .Find(session, x => x.Id == "main")
          .FirstAsync();

      if (state.PendingServedCount < maxPending)
      {
        var pending = await _talons.FindOneAndDeleteAsync<TalonDocument>(
            session,
            filter: x => x.PendingTime != null &&
                    x.PendingTime <= threshold,
            options: new FindOneAndDeleteOptions<TalonDocument>
            {
              Sort = Builders<TalonDocument>.Sort.Ascending(x => x.PendingTime),
            });

        if (pending != null)
        {
          await _queueState.UpdateOneAsync(
              session,
              x => x.Id == "main",
              Builders<QueueState>.Update.Inc(x => x.PendingServedCount, 1)
          );

          await session.CommitTransactionAsync();
          return pending.Number;
        }
      }

      var fifo = await _talons.FindOneAndDeleteAsync<TalonDocument>(
          filter: Builders<TalonDocument>.Filter.Empty,
          options: new FindOneAndDeleteOptions<TalonDocument>
          {
            Sort = Builders<TalonDocument>.Sort.Ascending(x => x.CreatedAt),
          });

      if (fifo != null)
      {
        await _queueState.UpdateOneAsync(
            session,
            x => x.Id == "main",
            Builders<QueueState>.Update.Set(x => x.PendingServedCount, 0)
        );
      }

      await session.CommitTransactionAsync();
      return fifo?.Number;
    }
    catch
    {
      await session.AbortTransactionAsync();
      throw;
    }
  }
  public Task AddWindowAsync(string windowNumber)
  {
    return _windows.InsertOneAsync(new WindowDocument
    {
      Number = windowNumber,
      CreatedAt = DateTime.UtcNow
    });
  }
  public async Task<string?> GetWindowAsync()
  {
    var doc = await _windows.FindOneAndDeleteAsync(
        filter: FilterDefinition<WindowDocument>.Empty,
        options: new FindOneAndDeleteOptions<WindowDocument>
        {
          Sort = Builders<WindowDocument>.Sort.Ascending(x => x.CreatedAt)
        });
    return doc?.Number;
  }

  public Task RemoveWindowAsync(string windowNumber)
      => _windows.DeleteOneAsync(x => x.Number == windowNumber);
}
