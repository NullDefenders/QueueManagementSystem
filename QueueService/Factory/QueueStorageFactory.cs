using QueueService.Helper;
public static class QueueStorageFactory
{
  public static async Task<IQueueStorage> CreateAsync(IServiceProvider sp)
  {
    var config = sp.GetRequiredService<IConfiguration>();
    var type = config["QueueStorage:Type"];

    if (type == "Redis")
      return sp.GetRequiredService<RedisQueueStorage>();

    if (type == "Mongo")
    {
      var mongo = sp.GetRequiredService<MongoQueueStorage>();
      await mongo.InitializeAsync();
      return mongo;
    }

    throw new InvalidOperationException("Unknown QueueStorage type");
  }
}