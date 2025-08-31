namespace DirectoryService.Models.Entities
{
    public class Facility
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}