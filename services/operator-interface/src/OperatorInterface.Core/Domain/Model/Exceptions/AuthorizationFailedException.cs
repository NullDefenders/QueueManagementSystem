namespace OperatorInterface.Core.Domain.Model.Exceptions;

public class AuthorizationFailedException : OperatorDomainException
{
    public AuthorizationFailedException(string reason) 
        : base($"Authorization failed: {reason}") { }
}
