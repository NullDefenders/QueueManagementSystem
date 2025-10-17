using GetTicket.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GetTicket.Services
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _tickets;
        private readonly ISimpleRabbitMQSender _rabbitMQSender;
        private readonly ILogger<TicketService> _logger;

        public TicketService(
            IOptions<TicketDatabaseSettings> settings,
            ISimpleRabbitMQSender rabbitMQSender,
            ILogger<TicketService> logger
        ) {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _tickets = database.GetCollection<Ticket>(settings.Value.TicketsCollectionName);

            _rabbitMQSender = rabbitMQSender;
            _logger = logger;
        }

        public async Task<Ticket> GenerateTicketAsync(TicketRequest request)
        {
            var today = DateTime.Today;
            var todaysTickets = await _tickets.Find(ticket => ticket.IssuedAt >= today && ticket.IssuedAt < today.AddDays(1)).ToListAsync();
            int lastTicketNumber = todaysTickets
                .Where(ticket => ticket.TalonNumber.StartsWith(request.ServiceCode + "-"))
                .Select(ticket =>
                {
                    var parts = ticket.TalonNumber.Split('-');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                    {
                        return number;
                    }
                    return 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            int nextNumber = lastTicketNumber + 1;
            string newTicketTalonNumber = $"{request.ServiceCode}-{nextNumber}";

            var newTicket = new Ticket
            {
                TalonNumber = newTicketTalonNumber,
                ServiceName = request.ServiceName,
                IssuedAt = DateTime.UtcNow,
                ServiceCode = request.ServiceCode,
                PendingTime = null,
            };

            await _tickets.InsertOneAsync(newTicket);

            await _rabbitMQSender.SendTicketAsync(newTicket);

            return newTicket;
        }
    }
}