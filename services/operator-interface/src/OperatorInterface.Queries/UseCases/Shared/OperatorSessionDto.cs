using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Queries.UseCases.Shared;

public record OperatorSessionDto(
    SessionId SessionId,
    OperatorId OperatorId,
    WorkplaceCode WorkplaceCode,
    SessionStatus Status,
    DateTime? SessionStartTime,
    DateTime? SessionEndTime,
    List<ServiceInfo> AssignedServices,
    List<ClientSessionDto> ClientSessions,
    ClientSessionDto? CurrentClientSession
);