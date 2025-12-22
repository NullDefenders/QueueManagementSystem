namespace GetTicket.Models
{
    public class TicketDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string TicketsCollectionName { get; set; } = null!;
    }
}
