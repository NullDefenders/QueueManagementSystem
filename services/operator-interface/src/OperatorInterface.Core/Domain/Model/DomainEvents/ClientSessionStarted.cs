using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientSessionStarted(
    DateTime OccurredAt,
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime SessionStartTime
) : DomainEvent(OccurredAt, SessionId);