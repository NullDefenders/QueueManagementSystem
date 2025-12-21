using System.ComponentModel.DataAnnotations;

namespace QueueAdminPanel.Models
{
    /// <summary>
    /// DTO для ответа с информацией о настройках электронной очереди
    /// </summary>
    public class QueueSettingsResponseDto
    {
        public QueueSettingsResponseDto() { }

        public QueueSettingsResponseDto(
            Guid id,
            int minutesBeforePending,
            int minutesAfterPending,
            int pendingCount)
        {
            Id = id;
            MinutesBeforePending = minutesBeforePending;
            MinutesAfterPending = minutesAfterPending;
            PendingCount = pendingCount;
        }

        [Display(Name = "ID настроек электронной очереди")]
        public Guid Id { get; set; }

        /// <summary>
        /// За сколько минут до начала записи можно приглашать клиента с талоном
        /// </summary>
        [Display(Name = "За сколько минут до начала записи можно приглашать клиента с талоном")]
        public int MinutesBeforePending { get; set; }

        /// <summary>
        /// Время ожидания клиента по записи (в минутах) перед тем, как вызвать следующего
        /// </summary>
        [Display(Name = "Время ожидания клиента по записи (в минутах) перед тем, как вызвать следующего")]
        public int MinutesAfterPending { get; set; }

        /// <summary>
        /// Максимальное количество клиентов по записи, которое можно обслужить подряд перед обслуживанием клиента без записи
        /// </summary>
        [Display(Name = "Максимальное количество клиентов по записи, которое можно обслужить подряд перед обслуживанием клиента без записи")]
        public int PendingCount { get; set; }
    }
}
