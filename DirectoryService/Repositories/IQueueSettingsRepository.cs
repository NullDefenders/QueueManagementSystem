using DirectoryService.Models.DTOs;
using DirectoryService.Models.Entities;

namespace DirectoryService.Repositories
{
    public interface IQueueSettingsRepository
    {
        Task<QueueSettings> GetQueueSettingsAsync(Guid queueId);

        Task<QueueSettings> UpdateQueueSettingsAsync(Guid queueId, int minutesBeforePending, int minutesAfterPending, int PendingCount);

        Task<bool> QueueExistsAsync(Guid queryId);
    }
}
