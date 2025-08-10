using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record WorkSessionStarted(
    DateTime OccurredAt,
    SessionId SessionId,
    DateTime StartTime
) : DomainEvent(OccurredAt, SessionId);