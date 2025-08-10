namespace OperatorInterface.Core.Domain.SharedKernel;

public enum SessionStatus
{
    Authorized,
    ReadyToWork,
    WaitingAssignment,
    WaitingClient,
    ServingClient,
    Closed
}