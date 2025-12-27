namespace QueueService.Helper;

public interface IQueueStorage
{
    Task AddTalonAsync(string talonNumber, double? pendingTime);
    Task<string?> GetNextTalonAsync();

    Task AddWindowAsync(string windowNumber);
    Task<string?> GetWindowAsync();
    Task RemoveWindowAsync(string windowNumber);
}
