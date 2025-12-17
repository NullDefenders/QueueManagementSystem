using DirectoryService.Models.DTOs;

namespace DirectoryService.Services
{
    public interface IQueueSettingsService
    {
        Task<QueueSettingsResponseDto?> GetQueueSettingsAsync(Guid queueId);

        Task<QueueSettingsResponseDto> UpdateQueueSettingsAsync(
            Guid queueId, 
            int minutesBeforePending, 
            int minutesAfterPending, 
            int pendingCount);
    }
}