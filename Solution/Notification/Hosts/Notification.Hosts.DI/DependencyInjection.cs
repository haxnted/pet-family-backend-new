using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application;
using Notification.Infrastructure;

namespace Notification.Hosts.DI;

/// <summary>
/// Содержит все внедренные зависимости для Host приложений.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости для Host приложений.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddHostDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication()
            .AddInfrastructure(configuration);

        return services;
    }
}