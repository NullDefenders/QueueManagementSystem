using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientSessionCompleted(
    SessionId SessionId,
    TicketNumber TicketNumber,
    DateTime SessionEndTime,
    TimeSpan SessionDuration
) : DomainEvent;