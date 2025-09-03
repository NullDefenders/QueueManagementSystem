using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record OperatorSessionClosed(
    SessionId SessionId,
    DateTime EndTime,
    TimeSpan SessionDuration
) : DomainEvent;