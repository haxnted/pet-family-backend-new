using Microsoft.Extensions.DependencyInjection;

namespace Account.Services;

/// <summary>
/// Класс для настройки зависимостей Application слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет сервисы Application слоя в контейнер зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
