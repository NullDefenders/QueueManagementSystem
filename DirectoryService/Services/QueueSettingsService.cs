using DirectoryService.Models.DTOs;
using DirectoryService.Models.Entities;
using DirectoryService.Repositories;

namespace DirectoryService.Services
{
    public class QueueSettingsService : IQueueSettingsService
    {
        private readonly IQueueSettingsRepository _queueSettingsRepository;

        public QueueSettingsService(IQueueSettingsRepository queueSettingsRepository)
        {
            _queueSettingsRepository = queueSettingsRepository;
        }

        public async Task<QueueSettingsResponseDto?> GetQueueSettingsAsync(Guid queueId)
        {
            if (!await _queueSettingsRepository.QueueExistsAsync(queueId))
                return null;

            var queueSettings = await _queueSettingsRepository.GetQueueSettingsAsync(queueId);

            return new QueueSettingsResponseDto(
                id: queueSettings.Id,
                minutesBeforePending: queueSettings.MinutesBeforePending,
                minutesAfterPending: queueSettings.MinutesAfterPending,
                pendingCount: queueSettings.PendingCount);
        }

        public async Task<QueueSettingsResponseDto> UpdateQueueSettingsAsync(
            Guid queueId,
            int minutesBeforePending,
            int minutesAfterPending,
            int pendingCount)
        {
            if (!await _queueSettingsRepository.QueueExistsAsync(queueId))
                throw new ArgumentException($"Очередь с ID {queueId} не найдена");

            var updatedSettings = await _queueSettingsRepository.UpdateQueueSettingsAsync(
                queueId, minutesBeforePending, minutesAfterPending, pendingCount);

            return new QueueSettingsResponseDto(
                id: updatedSettings.Id,
                minutesBeforePending: updatedSettings.MinutesBeforePending,
                minutesAfterPending: updatedSettings.MinutesAfterPending,
                pendingCount: updatedSettings.PendingCount);
        }
    }
}
