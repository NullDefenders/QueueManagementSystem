/*
namespace OperatorInterface.BlazorApp.Application;

// Value Objects
public record OperatorId(string Value);
public record SessionId(Guid Value);
public record WorkplaceCode(string Value);

// Enums
public enum SessionStatus
{
    Authorized,
    ReadyToWork,
    WaitingAssignment,
    WaitingClient,
    ServingClient,
    Closed
}

public enum ClientSessionResult
{
    None,
    ServiceCompleted,
    ClientNotCame
}

// DTOs
public class OperatorSessionDto
{
    public SessionId SessionId { get; set; } = default!;
    public OperatorId OperatorId { get; set; } = default!;
    public WorkplaceCode WorkplaceCode { get; set; } = default!;
    public SessionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SessionStartTime { get; set; }
    public DateTime? SessionEndTime { get; set; }
    public List<ServiceInfo> AssignedServices { get; set; } = new();
    public List<ClientSessionDto> ClientSessions { get; set; } = new();
    public ClientSessionDto? CurrentClientSession => ClientSessions.FirstOrDefault(cs => cs.IsActive);
}

public class ClientSessionDto
{
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime AssignmentTime { get; set; }
    public DateTime? ServiceStartTime { get; set; }
    public DateTime? ServiceEndTime { get; set; }
    public ClientSessionResult Result { get; set; }
    public bool IsActive => Result == ClientSessionResult.None;
}

public class ServiceInfo
{
    public string ServiceCode { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class OperatorInfo
{
    public OperatorId OperatorId { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

// Domain Events
public abstract class DomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public class SessionCreatedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public OperatorId OperatorId { get; }

    public SessionCreatedEvent(SessionId sessionId, OperatorId operatorId)
    {
        SessionId = sessionId;
        OperatorId = operatorId;
    }
}

public class ClientAssignedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public string TicketNumber { get; }

    public ClientAssignedEvent(SessionId sessionId, string ticketNumber)
    {
        SessionId = sessionId;
        TicketNumber = ticketNumber;
    }
}

public class ClientServiceStartedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public string TicketNumber { get; }

    public ClientServiceStartedEvent(SessionId sessionId, string ticketNumber)
    {
        SessionId = sessionId;
        TicketNumber = ticketNumber;
    }
}

public class ClientServiceCompletedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public string TicketNumber { get; }
    public DateTime ServiceStartTime { get; }
    public DateTime ServiceEndTime { get; }
    public TimeSpan ServiceDuration => ServiceEndTime - ServiceStartTime;

    public ClientServiceCompletedEvent(SessionId sessionId, string ticketNumber, DateTime serviceStartTime, DateTime serviceEndTime)
    {
        SessionId = sessionId;
        TicketNumber = ticketNumber;
        ServiceStartTime = serviceStartTime;
        ServiceEndTime = serviceEndTime;
    }
}

public class ClientNotCameEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public string TicketNumber { get; }
    public string Reason { get; }

    public ClientNotCameEvent(SessionId sessionId, string ticketNumber, string reason)
    {
        SessionId = sessionId;
        TicketNumber = ticketNumber;
        Reason = reason;
    }
}

public class SessionClosedEvent : DomainEvent
{
    public SessionId SessionId { get; }
    public OperatorId OperatorId { get; }
    public DateTime? SessionStartTime { get; }
    public DateTime SessionEndTime { get; }
    public int TotalClients { get; }
    public int CompletedClients { get; }
    public int NotCameClients { get; }
    public TimeSpan? WorkDuration => SessionStartTime.HasValue ? SessionEndTime - SessionStartTime.Value : null;

    public SessionClosedEvent(
        SessionId sessionId,
        OperatorId operatorId,
        DateTime? sessionStartTime,
        DateTime sessionEndTime,
        int totalClients,
        int completedClients,
        int notCameClients)
    {
        SessionId = sessionId;
        OperatorId = operatorId;
        SessionStartTime = sessionStartTime;
        SessionEndTime = sessionEndTime;
        TotalClients = totalClients;
        CompletedClients = completedClients;
        NotCameClients = notCameClients;
    }
}

public class AuthorizationFailedException : Exception
{
    public AuthorizationFailedException(string message) : base(message) { }
}
*/