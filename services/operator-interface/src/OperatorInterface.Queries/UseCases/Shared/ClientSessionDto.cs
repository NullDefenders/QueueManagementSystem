using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Queries.UseCases.Shared;

public record ClientSessionDto(
    string TicketNumber,
    DateTime AssignmentTime,
    DateTime? ServiceStartTime,
    DateTime? ServiceEndTime,
    ClientSessionResult? Result,
    bool IsActive,
    bool IsCompleted
);