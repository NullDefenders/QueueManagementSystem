using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientSessionStarted(
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime SessionStartTime
) : DomainEvent;