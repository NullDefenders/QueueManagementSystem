using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientSessionCompleted(
    DateTime OccurredAt,
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime SessionEndTime,
    TimeSpan SessionDuration
) : DomainEvent(OccurredAt, SessionId);