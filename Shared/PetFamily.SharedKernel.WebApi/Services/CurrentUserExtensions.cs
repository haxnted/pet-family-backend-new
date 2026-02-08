using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.WebApi.Services;

/// <summary>
/// Extension методы для регистрации ICurrentUser.
/// </summary>
public static class CurrentUserExtensions
{
    /// <summary>
    /// Добавляет сервис ICurrentUser на основе HttpContext.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
        return services;
    }
}