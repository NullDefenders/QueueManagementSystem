using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Domain.Model.Exceptions;

public class InvalidSessionStateException : OperatorDomainException
{
    public InvalidSessionStateException(string operation, SessionStatus currentStatus) 
        : base($"Cannot perform operation '{operation}' in current session status '{currentStatus}'") { }
}
