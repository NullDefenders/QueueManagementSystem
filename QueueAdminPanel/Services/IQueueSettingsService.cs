using QueueAdminPanel.Models;

namespace QueueAdminPanel.Services
{
    public interface IQueueSettingsService
    {
        Task<QueueSettingsResponseDto> GetQueueSettingsAsync(Guid queueId);
        Task<bool> UpdateQueueSettingsAsync(Guid queueId, UpdateQueueSettingsRequestDto dto);
    }
}
