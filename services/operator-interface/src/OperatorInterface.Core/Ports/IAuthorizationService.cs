using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Ports;

public interface IAuthorizationService
{
    Task<(bool IsSuccess, string? ErrorMessage, List<ServiceInfo> AssignedServices)> 
        AuthorizeOperatorAsync(Credentials credentials);
}