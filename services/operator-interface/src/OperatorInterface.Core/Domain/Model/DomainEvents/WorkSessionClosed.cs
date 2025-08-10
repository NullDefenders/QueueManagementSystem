using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record WorkSessionClosed(
    DateTime OccurredAt,
    SessionId SessionId,
    DateTime EndTime,
    TimeSpan SessionDuration
) : DomainEvent(OccurredAt, SessionId);