using QueueAdminPanel.Models;
using QueueAdminPanel.Services;

namespace QueueAdminPanel.Services
{
    public class QueueSettingsService : IQueueSettingsService
    {
        private readonly HttpClient _httpClient;

        public QueueSettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<QueueSettingsResponseDto> GetQueueSettingsAsync(Guid queueId)
        {
            var response = await _httpClient.GetFromJsonAsync<QueueSettingsResponseDto>($"api/queues/{queueId}/settings");
            return response ?? throw new InvalidOperationException("Failed to load queue settings.");
        }

        public async Task<bool> UpdateQueueSettingsAsync(Guid queueId, UpdateQueueSettingsRequestDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/queues/{queueId}/settings", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
