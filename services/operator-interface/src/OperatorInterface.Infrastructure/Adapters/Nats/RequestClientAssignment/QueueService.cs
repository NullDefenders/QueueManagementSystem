using OperatorInterface.Core.Domain.SharedKernel;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Infrastructure.Adapters.Nats.RequestClientAssignment;

public sealed class QueueService : IQueueService
{
    public Task RequestClientAssignmentAsync(OperatorId operatorId, WorkplaceCode workplaceCode)
    {
        return Task.CompletedTask;
    }
}