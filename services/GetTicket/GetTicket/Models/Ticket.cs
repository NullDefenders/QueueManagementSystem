using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GetTicket.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required int Id { get; set; }

        public required string TicketNumber { get; set; }

        public required DateTime IssuedAt { get; set; }

        public string ServiceName { get; set; } = "";
    }
}
