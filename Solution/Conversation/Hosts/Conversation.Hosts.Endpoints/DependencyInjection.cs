using System.Reflection;
using Conversation.Hosts.DI;
using PetFamily.SharedKernel.WebApi.Extensions;
using PetFamily.SharedKernel.WebApi.Services;

namespace Conversation.Hosts.Endpoints;

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
                title: "Conversation API",
                description: "API для управления чатами и сообщениями",
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
            .AddDatabaseHealthCheck(configuration, "ConversationDbContext")
            .AddRabbitMqHealthCheck(configuration, "RabbitMQ")
            .AddKeycloakHealthCheck(configuration, "Keycloak");

        return services;
    }

    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    private static IServiceCollection ConfigureAuthentification(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddKeycloakJwtBearer(configuration);
        services.AddDefaultAuthorizationPolicies();

        return services;
    }
}
