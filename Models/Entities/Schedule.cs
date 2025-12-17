namespace DirectoryService.Models.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public Department Department { get; set; }


        public string Name { get; set; }
        public bool IsActive { get; set; }

        public TimeSpan WorkDayStart { get; set; }
        public TimeSpan WorkDayEnd { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public ICollection<NonWorkingPeriod> NonWorkingPeriods { get; set; } = new List<NonWorkingPeriod>();

        public class NonWorkingPeriod
        {
            public Guid Id { get; set; }
            public Guid ScheduleId { get; set; }
            public Schedule Schedule { get; set; }

            public DayOfWeek DayOfWeek { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}
