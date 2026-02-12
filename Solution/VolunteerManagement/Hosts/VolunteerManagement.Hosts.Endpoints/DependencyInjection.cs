using System.Reflection;
using PetFamily.SharedKernel.WebApi.Diagnostics;
using PetFamily.SharedKernel.WebApi.Extensions;
using PetFamily.SharedKernel.WebApi.Services;
using VolunteerManagement.Hosts.DI;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Hosts.Endpoints;

/// <summary>
/// Класс для настройки зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Внедрение зависимостей всего приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddGlobalErrorHandling();
        services.AddHostDependencies(configuration)
            .AddSwaggerGenWithJwt(
                title: "VolunteerManagement API",
                description: "API для управления волонтёрами и питомцами",
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
            .AddDatabaseHealthCheck(configuration, "VolunteerDbContext")
            .AddRabbitMqHealthCheck(configuration, "RabbitMQ")
            .AddKeycloakHealthCheck(configuration, "Keycloak");

        services.AddMassTransitPublisher(configuration);

        services.AddOpenTelemetryTracing(
            configuration,
            serviceName: DiagnosticNames.VolunteerManagement,
            serviceVersion: "1.0.0",
            additionalActivitySources: "Wolverine");
        return services;
    }

    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    private static IServiceCollection ConfigureAuthentification(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddKeycloakJwtBearer(configuration);
        services.AddDefaultAuthorizationPolicies();

        return services;
    }
}