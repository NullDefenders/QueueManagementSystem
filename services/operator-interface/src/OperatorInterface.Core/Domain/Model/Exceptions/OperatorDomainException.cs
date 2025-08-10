namespace OperatorInterface.Core.Domain.Model.Exceptions;

public class OperatorDomainException : Exception
{
    public OperatorDomainException(string message) : base(message) { }
    public OperatorDomainException(string message, Exception innerException) : base(message, innerException) { }
}
