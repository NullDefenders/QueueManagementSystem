using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.Exceptions;

public class SessionNotFoundException(SessionId sessionId)
    : OperatorDomainException($"Session with ID {sessionId} not found");