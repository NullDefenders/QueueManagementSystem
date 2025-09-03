using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientAssigned(
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime AssignmentTime
) : DomainEvent;