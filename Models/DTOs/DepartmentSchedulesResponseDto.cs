using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией о расписании подразделения
    /// </summary>
    public class DepartmentSchedulesResponseDto
    {
        /// <summary>
        /// Конструктор по умолчанию (для Swagger)
        /// </summary>
        public DepartmentSchedulesResponseDto() { }

        /// <summary>
        /// Конструктор с параметрами (если понадобится)
        /// </summary>
        public DepartmentSchedulesResponseDto(List<ScheduleDto> schedules)
        {
            Schedules = schedules;
        }

        /// <summary>
        /// Активные расписания подразделения
        /// </summary>
        [Display(Name = "Расписание подразделения")]
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();

        /// <summary>
        /// Информация о расписании
        /// </summary>
        public class ScheduleDto
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            public ScheduleDto() { }

            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public TimeSpan WorkDayStart { get; set; }
            public TimeSpan WorkDayEnd { get; set; }

            public List<NonWorkingPeriodDto> NonWorkingPeriods { get; set; } = new List<NonWorkingPeriodDto>();
        }

        /// <summary>
        /// Нерабочие периоды в расписании
        /// </summary>
        public class NonWorkingPeriodDto
        {
            /// <summary>
            /// Конструктор по умолчанию
            /// </summary>
            public NonWorkingPeriodDto() { }

            public Guid Id { get; set; }
            public DayOfWeek DayOfWeek { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}