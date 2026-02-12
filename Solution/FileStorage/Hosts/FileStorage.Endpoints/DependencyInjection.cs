using System.Reflection;
using FileStorage.DI;
using PetFamily.SharedKernel.WebApi.Extensions;
using PetFamily.SharedKernel.WebApi.Services;

namespace FileStorage.Endpoints;

/// <summary>
/// Регистрация зависимостей API слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости для Host приложений.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddGlobalErrorHandling();
        services.AddHostDependencies(configuration)
            .AddSwaggerGenWithJwt(
                title: "FileStorage API",
                description: "API для работы с файлами через MinIO",
                configureOptions: options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                })
            .AddDefaultAuthorizationPolicies()
            .AddCurrentUser()
            .ConfigureAuthentification(configuration);

        services.AddStandardHealthChecks()
            .AddRabbitMqHealthCheck(configuration, "RabbitMQ")
            .AddKeycloakHealthCheck(configuration, "Keycloak");

        return services;
    }

    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    private static void ConfigureAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKeycloakJwtBearer(configuration);
        services.AddDefaultAuthorizationPolicies();
    }
}