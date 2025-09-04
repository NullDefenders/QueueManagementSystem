using Microsoft.AspNetCore.Components;
using OperatorInterface.BlazorApp.Services;

namespace OperatorInterface.BlazorApp.Components;

/// <summary>
/// Базовый компонент для страниц, требующих авторизации
/// </summary>
public abstract class AuthenticatedPageComponent : ComponentBase
{
    [Inject] protected SessionStorageService SessionStorage { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;

    protected bool IsSessionReady { get; private set; } = false;

    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // SessionStorage уже инициализирован в MainLayout
            // Просто проверяем авторизацию
            if (!SessionStorage.IsLoggedIn)
            {
                Navigation.NavigateTo("/login", replace: true);
                return;
            }

            IsSessionReady = true;
            StateHasChanged();

            // Вызываем хук для специфичной логики страницы
            await OnSessionReadyAsync();
        }
    }

    /// <summary>
    /// Вызывается после того как сессия готова и пользователь авторизован
    /// </summary>
    protected virtual Task OnSessionReadyAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Компонент для отображения во время проверки авторизации
    /// </summary>
    protected RenderFragment RenderAuthCheck() => builder =>
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "d-flex justify-content-center align-items-center");
        builder.AddAttribute(2, "style", "height: 60vh;");

        builder.OpenElement(3, "div");
        builder.AddAttribute(4, "class", "text-center");

        builder.OpenElement(5, "div");
        builder.AddAttribute(6, "class", "spinner-border text-primary mb-3");
        builder.AddAttribute(7, "role", "status");

        builder.OpenElement(8, "span");
        builder.AddAttribute(9, "class", "visually-hidden");
        builder.AddContent(10, "Проверка авторизации...");
        builder.CloseElement();

        builder.CloseElement();

        builder.OpenElement(11, "p");
        builder.AddAttribute(12, "class", "text-muted");
        builder.AddContent(13, "Проверка авторизации...");
        builder.CloseElement();

        builder.CloseElement();
        builder.CloseElement();
    };
}