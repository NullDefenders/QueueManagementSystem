namespace DirectoryService.Models.Entities
{
    public class Department
    {
        public Guid Id { get; set; }

        public Guid FacilityId { get; set; }

        public Facility Facility { get; set; }


        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool AllowScheduledAppointments { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        // навигационное свойство для рабочих мест
        public ICollection<Workplace> Workplaces { get; set; } = new List<Workplace>();
        // навигационное свойство для категорий услуг
        public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
        // для расписаний
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
