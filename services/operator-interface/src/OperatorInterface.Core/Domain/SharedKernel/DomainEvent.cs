namespace OperatorInterface.Core.Domain.SharedKernel;

public abstract record DomainEvent(DateTime OccurredAt, SessionId SessionId);