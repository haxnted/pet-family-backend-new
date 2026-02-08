using System.Reflection;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Infrastructure.Data;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using PetFamily.SharedKernel.WebApi.Diagnostics;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace Auth.Endpoints;

/// <summary>
/// Класс для настройки зависимостей в приложении.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости в приложение.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer()
            .AddSwaggerGenWithJwt(
                title: "Auth API",
                description: "Auth API для работы с пользователями",
                configureOptions: options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                })
            .AddMassTransitWithOutbox<AuthDbContext>(
                configuration,
                configureBus: null,
                configureRabbitMq: (_, cfg) => { cfg.Message<UserCreatedEvent>(e => e.SetEntityName("auth-events")); })
            .ConfigureAuthentification(configuration)
            .AddApplication()
            .AddInfrastructure(configuration)
            .AddAllowAllCors()
            .AddGlobalErrorHandling();

        services.AddStandardHealthChecks()
            .AddDatabaseHealthCheck(configuration, "AuthDbContext")
            .AddRabbitMqHealthCheck(configuration, "RabbitMQ")
            .AddKeycloakHealthCheck(configuration, "Keycloak");
        
        services.AddOpenTelemetryTracing(
            configuration,
            serviceName: DiagnosticNames.Auth,
            serviceVersion: "1.0.0");
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