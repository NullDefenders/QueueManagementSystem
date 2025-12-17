namespace DirectoryService.Models.Entities
{
    public class Workplace
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
