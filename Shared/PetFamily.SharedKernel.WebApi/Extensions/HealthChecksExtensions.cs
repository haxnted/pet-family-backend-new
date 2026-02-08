using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки HealthChecks.
/// </summary>
public static class HealthChecksExtensions
{
    /// <summary>
    /// Добавляет базовые health checks для приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>IHealthChecksBuilder для добавления дополнительных проверок.</returns>
    public static IHealthChecksBuilder AddStandardHealthChecks(this IServiceCollection services)
    {
        return services.AddHealthChecks();
    }

    /// <summary>
    /// Добавляет health check для PostgreSQL базы данных.
    /// </summary>
    /// <param name="builder">IHealthChecksBuilder.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="sectionName">Название секции в конфигурации.</param>
    /// <returns>IHealthChecksBuilder для цепочки вызовов.</returns>
    public static IHealthChecksBuilder AddDatabaseHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string sectionName)
    {
        var connectionString = configuration.GetConnectionString(sectionName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{sectionName}' not found in configuration.");
        }

        return builder.AddNpgSql(
            connectionString,
            failureStatus: HealthStatus.Unhealthy,
            tags: ["db", "postgres"],
            timeout: TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Добавляет health check для RabbitMQ.
    /// </summary>
    /// <param name="builder">IHealthChecksBuilder.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="sectionName">Название секции в конфигурации.</param>
    /// <returns>IHealthChecksBuilder для цепочки вызовов.</returns>
    public static IHealthChecksBuilder AddRabbitMqHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string sectionName)
    {
        var section = configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");
        }

        var host = section.GetValue<string>("Host");
        var port = section.GetValue<int?>("Port") ?? 5672;
        var virtualHost = section.GetValue<string>("VirtualHost") ?? "/";
        var username = section.GetValue<string>("Username") ?? "guest";
        var password = section.GetValue<string>("Password") ?? "guest";

        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException($"RabbitMQ host is not configured in section '{sectionName}'.");
        }

        var connectionString =
            $"amqp://{username}:{password}@{host}:{port}{virtualHost}";

        return builder.AddRabbitMQ(
            async _ =>
            {
                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
                return await factory.CreateConnectionAsync();
            },
            name: "rabbitmq",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["messaging", "rabbitmq"],
            timeout: TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Добавляет health check для MinIO object storage.
    /// </summary>
    /// <param name="builder">IHealthChecksBuilder.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="sectionName">Название секции в конфигурации.</param>
    /// <returns>IHealthChecksBuilder для цепочки вызовов.</returns>
    public static IHealthChecksBuilder AddMinioHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string sectionName)
    {
        var section = configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");
        }

        var endpoint = section.GetValue<string>("Endpoint");
        var secure = section.GetValue<bool>("Secure");

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new InvalidOperationException($"MinIO endpoint is not configured in section '{sectionName}'.");
        }

        var scheme = secure ? "https" : "http";
        var healthUri = new Uri($"{scheme}://{endpoint}/minio/health/live");

        return builder.AddUrlGroup(
            uri: healthUri,
            failureStatus: HealthStatus.Unhealthy,
            tags: ["storage", "minio"],
            timeout: TimeSpan.FromSeconds(5));
    }

    /// <summary>
    /// Добавляет health check для Keycloak.
    /// </summary>
    /// <param name="builder">IHealthChecksBuilder.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="sectionName">Название секции конфигурации. По умолчанию "Keycloak".</param>
    /// <returns>IHealthChecksBuilder для цепочки вызовов.</returns>
    public static IHealthChecksBuilder AddKeycloakHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration,
        string sectionName)
    {
        var section = configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");
        }

        var baseUrl = section.GetValue<string>("Url");
        var realm = section.GetValue<string>("Realm");

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException($"Keycloak Url is not configured in section '{sectionName}'.");
        }

        if (string.IsNullOrWhiteSpace(realm))
        {
            throw new InvalidOperationException($"Keycloak Realm is not configured in section '{sectionName}'.");
        }

        var healthUri = new Uri($"{baseUrl}/realms/{realm}/.well-known/openid-configuration");

        return builder.AddUrlGroup(
            uri: healthUri,
            failureStatus: HealthStatus.Degraded,
            tags: ["auth", "keycloak"],
            timeout: TimeSpan.FromSeconds(5));
    }


    /// <summary>
    /// Настраивает endpoints для health checks.
    /// </summary>
    /// <param name="app">WebApplication.</param>
    /// <returns>WebApplication для цепочки вызовов.</returns>
    public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        timestamp = DateTime.UtcNow
                    }));
            }
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/db", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("db"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks("/health/messaging", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("messaging"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}