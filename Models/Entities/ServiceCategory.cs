namespace DirectoryService.Models.Entities
{
    public class ServiceCategory
    {
        public Guid Id { get; set; }


        public string Code { get; set; }

        public string Name  { get; set; }

        public string Prefix { get; set; }

        public bool AllowScheduledAppointments { get; set; }

        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public ICollection<Department> Departments { get; set; } = new List<Department>();

        public ICollection<Workplace> Workplaces { get; set; } = new List<Workplace>();

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
