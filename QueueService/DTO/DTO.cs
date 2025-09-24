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

