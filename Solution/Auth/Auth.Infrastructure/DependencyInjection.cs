using Auth.Infrastructure.Data;
using Auth.Infrastructure.Services;
using Auth.Infrastructure.Services.Keycloak;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.Infrastructure.Options;
using Polly;
using Polly.Extensions.Http;

namespace Auth.Infrastructure;

/// <summary>
/// Регистрация сервисов Infrastructure слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрация сервисов Infrastructure слоя.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.SectionName));

        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AuthDbContext")));

        services.AddScoped<IMigrator, AuthMigrator>();
        services.AddScoped<DevDataSeeder>();

        ConfigureKeycloak(services, configuration);

        return services;
    }


    /// <summary>
    /// Сконфигурировать Keycloak.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается если конфигурация Keycloak не найдена.
    /// </exception>
    private static void ConfigureKeycloak(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection(KeycloakOptions.SectionName));

        var keycloakConfig = configuration.GetSection(KeycloakOptions.SectionName).Get<KeycloakOptions>()
                             ?? throw new InvalidOperationException("Keycloak configuration is missing");

        services.AddAccessTokenManagement(options =>
        {
            options.Client.Clients.Add("keycloak-admin", new ClientCredentialsTokenRequest
            {
                Address = keycloakConfig.TokenEndpoint,
                ClientId = keycloakConfig.ClientId,
                ClientSecret = keycloakConfig.ClientSecret
            });
        });

        services.AddHttpClient("Keycloak")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddHttpClient<IKeycloakUserManagementClient, KeycloakUserManagementClient>()
            .AddClientAccessTokenHandler("keycloak-admin")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

        services.AddScoped<IKeycloakAuthorizationClient, KeycloakAuthorizationClient>();

        services.AddScoped<IKeycloakService, KeycloakServiceAdapter>();
    }

    /// <summary>
    /// Политика повторных попыток для HTTP запросов.
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    /// <summary>
    /// Политика Circuit Breaker для HTTP запросов.
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}