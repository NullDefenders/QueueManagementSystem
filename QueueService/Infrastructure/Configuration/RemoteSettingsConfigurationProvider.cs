using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueService.Infrastructure.Configuration;
using System.Text.Json;

public sealed class RemoteSettingsConfigurationProvider : ConfigurationProvider
{
    private readonly IConfiguration _baseConfiguration;
    private readonly ILogger? _logger;
    private readonly HttpClient _httpClient = new();

    public RemoteSettingsConfigurationProvider(
        IConfiguration baseConfiguration,
        ILoggerFactory? loggerFactory)
    {
        _baseConfiguration = baseConfiguration;
        _logger = loggerFactory?.CreateLogger<RemoteSettingsConfigurationProvider>();
    }

    public override void Load()
    {
        var url = _baseConfiguration["Settings:GetSettingURL"];
        _logger?.LogInformation("Geting Settings: {url}", url);
        if (string.IsNullOrWhiteSpace(url))
            return;

        try
        {
            var json = _httpClient.GetStringAsync(url)
                                  .GetAwaiter()
                                  .GetResult();

            _logger?.LogInformation("Get Settings: {Json}", json);

            var settings = JsonSerializer.Deserialize<RemoteSettingsDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (settings == null)
                return;

            Data["Settings:MinutesBeforePending"] = settings.MinutesBeforePending.ToString();
            Data["Settings:MinutesAfterPending"] = settings.MinutesAfterPending.ToString();
            Data["Settings:PendingCount"] = settings.PendingCount.ToString();
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to load remote settings, fallback to appsettings.json");
        }
    }
}
