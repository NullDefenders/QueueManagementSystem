namespace OperatorInterface.BlazorApp.Services;

public class NotificationService
{
    public event Action<string, NotificationType>? OnNotification;

    public async Task ShowSuccessAsync(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Success);
    }

    public async Task ShowErrorAsync(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Error);
    }

    public async Task ShowInfoAsync(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Info);
    }

    public async Task ShowWarningAsync(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Warning);
    }
}

public enum NotificationType
{
    Success,
    Error,
    Info,
    Warning
}