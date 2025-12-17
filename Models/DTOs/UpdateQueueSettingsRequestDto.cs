namespace DirectoryService.Models.DTOs
{
    public class UpdateQueueSettingsRequestDto
    {
        public int MinutesBeforePending { get; set; }
        public int MinutesAfterPending { get; set; }
        public int PendingCount { get; set; }
    }
}
