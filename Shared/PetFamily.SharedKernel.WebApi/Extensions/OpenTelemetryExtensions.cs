using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Расширения для настройки OpenTelemetry Distributed Tracing
/// </summary>
public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Добавить OpenTelemetry distributed tracing с экспортом в Jaeger
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="configuration">Конфигурация приложения</param>
    /// <param name="serviceName">Название микросервиса для идентификации в Jaeger</param>
    /// <param name="serviceVersion">Версия сервиса</param>
    /// <param name="additionalActivitySources">Дополнительные Activity Sources для трассировки</param>
    public static IServiceCollection AddOpenTelemetryTracing(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName,
        string serviceVersion = "1.0.0",
        params string[] additionalActivitySources)
    {
        var jaegerEndpoint = configuration["OpenTelemetry:Jaeger:Endpoint"] ?? "http://localhost:4317";
        var enableConsoleExporter = configuration.GetValue<bool>("OpenTelemetry:EnableConsoleExporter");

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: serviceName,
                    serviceVersion: serviceVersion,
                    serviceInstanceId: Environment.MachineName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag("http.method", request.Method);
                            activity.SetTag("http.url", request.Path);
                            activity.SetTag("http.user_agent", request.Headers.UserAgent.ToString());
                        };
                        options.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.SetTag("http.status_code", response.StatusCode);
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequestMessage = (activity, request) =>
                        {
                            activity.SetTag("http.request.method", request.Method.ToString());
                            activity.SetTag("http.request.url", request.RequestUri?.ToString());
                        };
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            activity.SetTag("db.operation", command.CommandText);
                        };
                    })
                    .AddSource(serviceName);

                foreach (var activitySource in additionalActivitySources)
                {
                    tracing.AddSource(activitySource);
                }

                tracing.AddSource("MassTransit");

                tracing.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(jaegerEndpoint);
                    options.Protocol = OtlpExportProtocol.Grpc;
                });

                if (enableConsoleExporter)
                {
                    tracing.AddConsoleExporter();
                }
            });

        return services;
    }

    /// <summary>
    /// Создать ActivitySource для микросервиса
    /// </summary>
    /// <param name="serviceName">Название сервиса</param>
    /// <param name="version">Версия сервиса</param>
    public static ActivitySource CreateActivitySource(string serviceName, string version = "1.0.0")
    {
        return new ActivitySource(serviceName, version);
    }

    /// <summary>
    /// Начать новый span для трассировки операции
    /// </summary>
    /// <param name="activitySource">Activity Source</param>
    /// <param name="operationName">Название операции</param>
    /// <param name="kind">Тип активности</param>
    /// <returns>Activity для добавления тегов и событий</returns>
    public static Activity? StartActivity(
        this ActivitySource activitySource,
        string operationName,
        ActivityKind kind = ActivityKind.Internal)
    {
        return activitySource.StartActivity(operationName, kind);
    }

    /// <summary>
    /// Добавить тег с информацией о пользователе
    /// </summary>
    public static Activity? AddUserTag(this Activity? activity, Guid userId)
    {
        activity?.SetTag("user.id", userId.ToString());
        return activity;
    }

    /// <summary>
    /// Добавить тег с информацией о сущности
    /// </summary>
    public static Activity? AddEntityTag(this Activity? activity, string entityType, Guid entityId)
    {
        activity?.SetTag("entity.type", entityType);
        activity?.SetTag("entity.id", entityId.ToString());
        return activity;
    }

    /// <summary>
    /// Добавить событие в trace
    /// </summary>
    public static Activity? AddTraceEvent(this Activity? activity, string eventName, params (string key, object? value)[] tags)
    {
        if (activity == null) return null;

        var tagsCollection = new ActivityTagsCollection();
        foreach (var (key, value) in tags)
        {
            tagsCollection.Add(key, value);
        }

        activity.AddEvent(new ActivityEvent(eventName, tags: tagsCollection));
        return activity;
    }

    /// <summary>
    /// Отметить активность как ошибочную
    /// </summary>
    public static Activity? RecordException(this Activity? activity, Exception exception)
    {
        if (activity == null) return null;

        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
        activity.RecordException(exception);
        return activity;
    }
}
