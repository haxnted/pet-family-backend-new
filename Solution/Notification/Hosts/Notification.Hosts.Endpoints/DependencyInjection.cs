using System.Reflection;
using Notification.Hosts.DI;
using PetFamily.SharedKernel.WebApi.Diagnostics;
using PetFamily.SharedKernel.WebApi.Extensions;
using PetFamily.SharedKernel.WebApi.Services;

namespace Notification.Hosts.Endpoints;

/// <summary>
/// Настройка зависимостей для приложения.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует все зависимости в приложении.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddProgramDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddGlobalErrorHandling();
        services.AddHostDependencies(configuration)
            .AddSwaggerGenWithJwt(
                title: "Notification API",
                description: "API для управления уведомлениями и предпочтениями пользователя.",
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
            .AddAllowAllCors()
            .ConfigureAuthentification(configuration);

        services.AddStandardHealthChecks()
            .AddDatabaseHealthCheck(configuration, "NotificationDbContext")
            .AddRabbitMqHealthCheck(configuration, "RabbitMQ")
            .AddKeycloakHealthCheck(configuration, "Keycloak");
        
        services.AddOpenTelemetryTracing(
            configuration,
            serviceName: DiagnosticNames.Notification,
            serviceVersion: "1.0.0");

        return services;
    }

    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    private static IServiceCollection ConfigureAuthentification(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddKeycloakJwtBearer(configuration);
        services.AddDefaultAuthorizationPolicies();

        return services;
    }
}