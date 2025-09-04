using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using OperatorInterface.Core.Domain.SharedKernel;

namespace OperatorInterface.BlazorApp.Services;

public class SessionStorageService
{
    private readonly ProtectedLocalStorage _protectedStorage;
    private readonly ILogger<SessionStorageService> _logger;
    private SessionId? _currentSessionId;
    private bool _isStorageLoaded = false;
    private readonly SemaphoreSlim _initSemaphore = new(1, 1);

    private const string SESSION_ID_KEY = "CurrentSessionId";

    public SessionStorageService(ProtectedLocalStorage protectedStorage, ILogger<SessionStorageService> logger)
    {
        _protectedStorage = protectedStorage;
        _logger = logger;
    }

    public SessionId? CurrentSessionId
    {
        get => _currentSessionId;
        set
        {
            if (_currentSessionId != value)
            {
                _currentSessionId = value;
                OnSessionChanged?.Invoke(value);
                _ = SaveSessionIdAsync(value);
            }
        }
    }

    public bool IsLoggedIn => CurrentSessionId != null;

    public event Action<SessionId?>? OnSessionChanged;

    public async Task InitializeAfterRenderAsync()
    {
        if (_isStorageLoaded) return;

        await _initSemaphore.WaitAsync();
        try
        {
            if (_isStorageLoaded) return;

            await LoadFromStorageAsync();
            _isStorageLoaded = true;
        }
        finally
        {
            _initSemaphore.Release();
        }
    }

    public void Clear()
    {
        CurrentSessionId = null;
    }

    public async Task ClearAsync()
    {
        _currentSessionId = null;

        if (_isStorageLoaded)
        {
            await _protectedStorage.DeleteAsync(SESSION_ID_KEY);
        }

        OnSessionChanged?.Invoke(null);
    }

    private async Task LoadFromStorageAsync()
    {
        try
        {
            _logger.LogInformation("Loading session from storage...");
            var sessionResult = await _protectedStorage.GetAsync<string>(SESSION_ID_KEY);
            _logger.LogInformation("Session result: {Success}, Value: {Value}", sessionResult.Success, sessionResult.Value);

            if (sessionResult.Success && !string.IsNullOrEmpty(sessionResult.Value))
            {
                if (Guid.TryParse(sessionResult.Value, out var sessionGuid))
                {
                    _currentSessionId = new SessionId(sessionGuid);
                }
            }

            // Trigger event after loading
            OnSessionChanged?.Invoke(_currentSessionId);
        }
        catch
        {
            // If there's any error reading from storage, start with clean state
            _currentSessionId = null;
        }
    }

    private async Task SaveSessionIdAsync(SessionId? sessionId)
    {
        if (!_isStorageLoaded) return;

        try
        {
            if (sessionId != null)
            {
                await _protectedStorage.SetAsync(SESSION_ID_KEY, sessionId.Value.ToString());
            }
            else
            {
                await _protectedStorage.DeleteAsync(SESSION_ID_KEY);
            }
        }
        catch
        {
            // Ignore storage errors - the in-memory value is still set
        }
    }

    public void Dispose()
    {
        _initSemaphore?.Dispose();
    }
}