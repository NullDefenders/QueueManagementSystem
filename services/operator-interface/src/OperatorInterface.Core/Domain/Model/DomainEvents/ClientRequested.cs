using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record ClientRequested(
    DateTime OccurredAt,
    SessionId SessionId
) : DomainEvent(OccurredAt, SessionId);