using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.DomainEvents;

public record OperatorSessionCreated(
    SessionId SessionId,
    OperatorId OperatorId,
    WorkplaceCode WorkplaceCode,
    List<ServiceInfo> AssignedServices
) : DomainEvent;