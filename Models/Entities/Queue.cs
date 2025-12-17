using System.Globalization;

namespace DirectoryService.Models.Entities
{
    public class Queue
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        public QueueSettings QueueSettings { get; set; }
    }
}
