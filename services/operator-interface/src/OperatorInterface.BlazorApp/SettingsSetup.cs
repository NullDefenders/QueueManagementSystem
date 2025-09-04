using Microsoft.Extensions.Options;
using OperatorInterface.Infrastructure;

namespace OperatorInterface.BlazorApp;

public class SettingsSetup : IConfigureOptions<Settings>
{
    private readonly IConfiguration _configuration;

    public SettingsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(Settings options)
    {
        options.ConnectionString = _configuration["CONNECTION_STRING"];
        options.AuthServiceGrpcHost = _configuration["AUTH_SERVICE_GRPC_HOST"];
    }
}