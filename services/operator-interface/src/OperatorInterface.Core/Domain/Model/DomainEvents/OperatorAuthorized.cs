using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record OperatorAuthorized(
    DateTime OccurredAt,
    SessionId SessionId,
    OperatorId OperatorId,
    WorkplaceCode WorkplaceCode,
    List<ServiceInfo> AssignedServices
) : DomainEvent(OccurredAt, SessionId);