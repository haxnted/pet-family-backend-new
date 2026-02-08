using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Services;

namespace Notification.Application;

/// <summary>
/// Класс для регистрации зависимостей из слоя Application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы слоя Application в контейнере зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<INotificationSettingsService, NotificationSettingsService>();
    }
}