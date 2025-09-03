using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.Core.Ports;

public interface IAuthorizationService
{
    Task<AuthResult> AuthorizeOperatorAsync(AuthRequest credentials, CancellationToken cancellationToken);
}

public record AuthRequest(string Login, string Password, string Workplace);

public class AuthResult
{
    public string Login { get; private set; }
    
    public string Workplace { get; private set; }
    
    public string FullName { get; private set; }
    public string WindowCode { get; private set; }
    public string WindowDisplayName { get; private set; }
    public string Location { get; private set; }

    public static AuthResult Create(string login, string workplace, string fullName, string windowCode,
        string windowDisplayName, string location)
    {
        return new AuthResult()
        {
            Login = login,
            Workplace = workplace,
            FullName = fullName,
            WindowCode = windowCode,
            WindowDisplayName = windowDisplayName,
            Location = location
        };
    }
}