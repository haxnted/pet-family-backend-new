using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки CORS.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Добавляет CORS политику, разрешающую все источники, методы и заголовки.
    /// Используйте только для разработки или внутренних API.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="policyName">Имя политики (опционально, если не указано - добавляется как default policy).</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddAllowAllCors(
        this IServiceCollection services,
        string? policyName = null)
    {
        services.AddCors(options =>
        {
            if (string.IsNullOrEmpty(policyName))
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            }
            else
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            }
        });

        return services;
    }

    /// <summary>
    /// Добавляет CORS политику с указанными источниками.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="origins">Разрешённые источники.</param>
    /// <param name="policyName">Имя политики (опционально).</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddCorsWithOrigins(
        this IServiceCollection services,
        string[] origins,
        string? policyName = null)
    {
        services.AddCors(options =>
        {
            if (string.IsNullOrEmpty(policyName))
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
            else
            {
                options.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }
        });

        return services;
    }
}