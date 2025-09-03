using Authorization.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OperatorInterface.Core.Ports;

namespace OperatorInterface.Infrastructure.Adapters.Grpc.AuthorizationService;

public class AuthorizationClient :  IAuthorizationService
{
    private readonly ILogger<AuthorizationClient> _logger;
    private readonly MethodConfig _methodConfig;
    private readonly SocketsHttpHandler _socketsHttpHandler;
    private readonly string _url;
    
    public AuthorizationClient(IOptions<Settings> options, ILogger<AuthorizationClient> logger)
    {
        if (string.IsNullOrWhiteSpace(options.Value.AuthServiceGrpcHost))
            throw new ArgumentException(nameof(options.Value.AuthServiceGrpcHost));
        
        _logger = logger;

        _url = options.Value.AuthServiceGrpcHost;
        
        _socketsHttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            EnableMultipleHttp2Connections = true
        };

        _methodConfig = new MethodConfig
        {
            Names = { MethodName.Default },
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = 5,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(5),
                BackoffMultiplier = 1.5,
                RetryableStatusCodes = { StatusCode.Unavailable }
            }
        };
    }

    public async Task<AuthResult> AuthorizeOperatorAsync(AuthRequest credentials, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Sending AuthRequest to endpoint {_url}");
        
        using var channel = GrpcChannel.ForAddress(_url, new GrpcChannelOptions
        {
            HttpHandler = _socketsHttpHandler,
            ServiceConfig = new ServiceConfig { MethodConfigs = { _methodConfig } }
        });

        var client = new Authorization.Grpc.OperatorAuthorizationService.OperatorAuthorizationServiceClient(channel);
        var result = await client.AuthorizeOperatorAsync(new AuthorizeOperatorRequest()
        {
            Workplace = credentials.Workplace,
            Login = credentials.Login,
            Password = credentials.Password,
            
        }, null, DateTime.UtcNow.AddSeconds(5), cancellationToken);
        
        return AuthResult.Create(
            result.Login,
            result.AssignedWorkplace,
            result.FullName,
            result.WindowAssignment.WindowCode,
            result.WindowAssignment.WindowDisplayName,
            result.WindowAssignment.Location);
    }
}