using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Ports;

public interface IQueueService
{
    Task RequestClientAssignmentAsync(OperatorId operatorId, WorkplaceCode workplaceCode);
}