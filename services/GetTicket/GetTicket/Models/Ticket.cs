using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GetTicket.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public required int Id { get; set; }

        public required string TalonNumber { get; set; }

        public required DateTime IssuedAt { get; set; }

        public DateTime PendingTime { get; set; }

        public required string ServiceName { get; set; } = "";

        public required string ServiceCode { get; set; } = "";
    }
}
