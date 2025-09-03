using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientDidNotCome(
    SessionId SessionId,
    TicketNumber TicketNumber,
    string Reason
) : DomainEvent;