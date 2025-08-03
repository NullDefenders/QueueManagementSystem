using GetTicket.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GetTicket.Services
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketService(IOptions<TicketDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _tickets = database.GetCollection<Ticket>(settings.Value.TicketsCollectionName);
        }

        public async Task<Ticket> GenerateTicketAsync()
        {
            // Получить последний талон по номеру
            var lastTicket = await _tickets.Find(_ => true)
                .SortByDescending(t => t.Id)
                .Limit(1)
                .FirstOrDefaultAsync();

            int nextNumber = (lastTicket?.Id ?? 0) + 1;

            var newTicket = new Ticket
            {
                Id = nextNumber,
                TicketNumber = nextNumber.ToString(),
                ServiceName = "",
                IssuedAt = DateTime.UtcNow,
            };

            await _tickets.InsertOneAsync(newTicket);
            return newTicket;
        }
    }
}