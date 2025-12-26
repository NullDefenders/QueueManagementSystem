using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public sealed class RemoteSettingsConfigurationSource : IConfigurationSource
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory? _loggerFactory;

    public RemoteSettingsConfigurationSource(
        IConfiguration configuration,
        ILoggerFactory? loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new RemoteSettingsConfigurationProvider(
            _configuration,
            _loggerFactory);
    }
}
