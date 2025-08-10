using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientAssigned(
    DateTime OccurredAt,
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime AssignmentTime
) : DomainEvent(OccurredAt, SessionId);