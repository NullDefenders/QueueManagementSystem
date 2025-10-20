using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GetTicket.Models
{
    public class Ticket
    {
        [BsonId]
        public required string TalonNumber { get; set; }

        public required DateTime IssuedAt { get; set; }

        public string? PendingTime { get; set; }

        public required string ServiceName { get; set; } = "";

        public required string ServiceCode { get; set; } = "";
    }
}
