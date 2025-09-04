using System.Threading.Channels;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.BlazorApp.Services;

public class EventProcessorService : BackgroundService
{
    //private readonly ChannelReader<DomainEvent> _eventReader;
    private readonly ILogger<EventProcessorService> _logger;

    public EventProcessorService(/*ChannelReader<DomainEvent> eventReader,*/ ILogger<EventProcessorService> logger)
    {
        //_eventReader = eventReader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        /*
        await foreach (var domainEvent in _eventReader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await ProcessEvent(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing domain event {EventType}", domainEvent.GetType().Name);
            }
        }
        */
    }

    private async Task ProcessEvent(DomainEvent domainEvent)
    {
        /*
        switch (domainEvent)
        {
            case SessionCreatedEvent sessionCreated:
                _logger.LogInformation("Session created: {SessionId} for operator {OperatorId}",
                    sessionCreated.SessionId.Value, sessionCreated.OperatorId.Value);
                break;

            case ClientAssignedEvent clientAssigned:
                _logger.LogInformation("Client {TicketNumber} assigned to session {SessionId}",
                    clientAssigned.TicketNumber, clientAssigned.SessionId.Value);
                break;

            case ClientServiceStartedEvent serviceStarted:
                _logger.LogInformation("Client service started: {TicketNumber} in session {SessionId}",
                    serviceStarted.TicketNumber, serviceStarted.SessionId.Value);
                break;

            case ClientServiceCompletedEvent serviceCompleted:
                _logger.LogInformation("Client service completed: {TicketNumber} in session {SessionId}, duration: {Duration}",
                    serviceCompleted.TicketNumber, serviceCompleted.SessionId.Value, serviceCompleted.ServiceDuration);
                break;

            case ClientNotCameEvent clientNotCame:
                _logger.LogWarning("Client did not come: {TicketNumber} in session {SessionId}, reason: {Reason}",
                    clientNotCame.TicketNumber, clientNotCame.SessionId.Value, clientNotCame.Reason);
                break;

            case SessionClosedEvent sessionClosed:
                _logger.LogInformation(
                    "Session closed: {SessionId} for operator {OperatorId}, duration: {Duration}, clients: {Total} (completed: {Completed}, not came: {NotCame})",
                    sessionClosed.SessionId.Value,
                    sessionClosed.OperatorId.Value,
                    sessionClosed.WorkDuration?.ToString(@"hh\:mm\:ss") ?? "unknown",
                    sessionClosed.TotalClients,
                    sessionClosed.CompletedClients,
                    sessionClosed.NotCameClients);
                break;

            default:
                _logger.LogInformation("Processed domain event: {EventType}", domainEvent.GetType().Name);
                break;
        }
        */
    }
}