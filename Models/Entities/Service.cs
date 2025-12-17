namespace DirectoryService.Models.Entities
{
    public class Service
    {
        public Guid Id { get; set; }

        public Guid ServiceCategoryId { get; set; }

        public ServiceCategory ServiceCategory { get; set; }


        public string Code { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public bool AllowScheduledAppointments { get; set; }

        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
