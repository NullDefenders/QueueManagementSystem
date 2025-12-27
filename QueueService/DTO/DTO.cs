using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QueueService.DTO;
public enum WindowsStatus { free, busy};
public partial class TalonDTO
{
  public string? TalonNumber { get; set; }
  public string? ServiceCode { get; set; }
  public TimeSpan? PendingTime { get; set; }
}

public partial class WindowDTO
{
  public string? WindowNumber { get; set; }
  public WindowsStatus? Status  { get; set; }
}

public partial class QueueDTO
{
  public required string WindowNumber { get; set; }
  public required string TalonNumber { get; set; }
}

public class TalonDocument
{
  public ObjectId Id { get; set; }
  public string Number { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public double? PendingTime { get; set; }
}
public class WindowDocument
{
  public ObjectId Id { get; set; }
  public string Number { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}

public class QueueState
{
  [BsonId]
  public string Id { get; set; } = "main";

  public int PendingServedCount { get; set; }
}
