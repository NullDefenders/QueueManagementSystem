namespace DirectoryService.Models.Entities
{
    public class QueueSettings
    {
        public Guid Id { get; set; }

        public Guid QueueId { get; set; }
        public Queue Queue { get; set; }

        /// <summary>
        /// За сколько минут до начала записи можно приглашать клиента с талоном
        /// </summary>
        public int MinutesBeforePending { get; set; }

        /// <summary>
        /// Время ожидания клиента по записи (в минутах) перед тем как вызвать следующего
        /// </summary>
        public int MinutesAfterPending { get; set;}

        /// <summary>
        /// Максимальное количество клиентов по записи, обслуживаемых подряд перед обслуживанием клиента без записи
        /// </summary>
        public int PendingCount { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
