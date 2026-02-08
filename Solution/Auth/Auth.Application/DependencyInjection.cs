using Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

/// <summary>
/// Регистрация сервисов Application слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрация сервисов Application слоя.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
