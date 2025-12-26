namespace QueueService.Infrastructure.Configuration;
public sealed class RemoteSettingsDto
{
    public Guid Id { get; set; }
    public int MinutesBeforePending { get; set; }
    public int MinutesAfterPending { get; set; }
    public int PendingCount { get; set; }
}
