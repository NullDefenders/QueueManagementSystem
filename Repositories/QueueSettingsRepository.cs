using DirectoryService.Data;
using DirectoryService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Repositories
{
    public class QueueSettingsRepository : IQueueSettingsRepository
    {
        private readonly ApplicationContext _dbContext;

        public QueueSettingsRepository(ApplicationContext context)
        {
            _dbContext = context;
        }

        public async Task<QueueSettings> GetQueueSettingsAsync(Guid queueId)
        {
            return await _dbContext.QueueSettings
                .Where(qs => qs.QueueId == queueId && qs.DeletedAt == null)
                .FirstAsync(); // first or default?
        }

        public async Task<QueueSettings> UpdateQueueSettingsAsync(Guid queueId, int minutesBeforePending, int minutesAfterPending, int pendingCount)
        {
            var existingSettings = await _dbContext.QueueSettings
                .Where(qs => qs.QueueId == queueId && qs.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (existingSettings == null)
            {
                var newSettings = new QueueSettings
                {
                    QueueId = queueId,
                    MinutesBeforePending = minutesBeforePending,
                    MinutesAfterPending = minutesAfterPending,
                    PendingCount = pendingCount
                };

                _dbContext.QueueSettings.Add(newSettings);
                await _dbContext.SaveChangesAsync();
                return newSettings;
            }
            else
            {
                existingSettings.MinutesBeforePending = minutesBeforePending;
                existingSettings.MinutesAfterPending = minutesAfterPending;
                existingSettings.PendingCount = pendingCount;

                await _dbContext.SaveChangesAsync();
                return existingSettings;
            }
        }

        public async Task<bool> QueueExistsAsync(Guid queryId)
        {
            return await _dbContext.Queues.AnyAsync(q => q.Id == queryId);
        }
    }
}
