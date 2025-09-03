using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record OperatorSessionOpened(
    SessionId SessionId,
    DateTime StartTime
) : DomainEvent;