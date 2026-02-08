using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки логирования с ELK Stack.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Настраивает Serilog с отправкой логов в Elasticsearch.
    /// </summary>
    /// <param name="builder">WebApplication builder.</param>
    /// <param name="applicationName">Название приложения для индексирования логов.</param>
    /// <returns>WebApplicationBuilder для цепочки вызовов.</returns>
    public static WebApplicationBuilder AddSerilogWithElk(
        this WebApplicationBuilder builder,
        string applicationName)
    {
        var configuration = builder.Configuration;
        var environment = builder.Environment;

        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning);

            loggerConfiguration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");

            var elasticsearchUrl = configuration["Elasticsearch:Url"] ?? "http://elasticsearch:9200";
            var indexFormat = configuration["Elasticsearch:IndexFormat"] ??
                              $"petfamily-{applicationName.ToLowerInvariant()}-{{0:yyyy.MM}}";

            loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                IndexFormat = indexFormat,
                NumberOfShards = 2,
                NumberOfReplicas = 1,
                MinimumLogEventLevel = environment.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information,
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
                ModifyConnectionSettings = x => x.BasicAuthentication("", "") // для xpack.security.enabled=false
            });
        });

        return builder;
    }

    /// <summary>
    /// Настраивает Serilog только с Console логированием (для сервисов без ELK).
    /// </summary>
    /// <param name="builder">WebApplication builder.</param>
    /// <param name="applicationName">Название приложения.</param>
    /// <returns>WebApplicationBuilder для цепочки вызовов.</returns>
    public static WebApplicationBuilder AddSerilogConsole(
        this WebApplicationBuilder builder,
        string applicationName)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", applicationName)
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");
        });

        return builder;
    }

    /// <summary>
    /// Добавляет Serilog request logging middleware.
    /// </summary>
    /// <param name="app">WebApplication.</param>
    /// <returns>WebApplication для цепочки вызовов.</returns>
    public static WebApplication UseSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.GetLevel = (_, elapsed, ex) => ex != null
                ? LogEventLevel.Error
                : elapsed > 5000
                    ? LogEventLevel.Warning
                    : LogEventLevel.Information;
            
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());

                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    diagnosticContext.Set("UserId", httpContext.User.FindFirst("sub")?.Value);
                }
            };
        });

        return app;
    }
}