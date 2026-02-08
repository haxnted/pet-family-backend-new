using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки MassTransit с RabbitMQ.
/// </summary>
public static class MassTransitExtensions
{
    /// <summary>
    /// Добавляет MassTransit с RabbitMQ транспортом.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="configureBus">Конфигурация consumers и endpoints.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    /// <remarks>
    /// Ожидаемая конфигурация в appsettings.json:
    /// <code>
    /// {
    ///   "RabbitMQ": {
    ///     "Host": "localhost",
    ///     "Port": 5672,
    ///     "VirtualHost": "/",
    ///     "Username": "guest",
    ///     "Password": "guest"
    ///   }
    /// }
    /// </code>
    /// </remarks>
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus = null)
    {
        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqUri = BuildRabbitMqUri(configuration);
                cfg.Host(rabbitMqUri);
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Добавляет MassTransit с RabbitMQ c расширенной конфигурацией.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="configureBus">Конфигурация consumers.</param>
    /// <param name="configureRabbitMq">Расширенная конфигурация RabbitMQ (для exchanges, endpoints).</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureRabbitMq)
    {
        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqUri = BuildRabbitMqUri(configuration);
                cfg.Host(rabbitMqUri);

                configureRabbitMq?.Invoke(context, cfg);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Строит URI для подключения к RabbitMQ из конфигурации.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>URI для подключения к RabbitMQ.</returns>
    private static Uri BuildRabbitMqUri(IConfiguration configuration)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
        var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
        var username = configuration["RabbitMQ:Username"] ?? "guest";
        var password = configuration["RabbitMQ:Password"] ?? "guest";

        return new Uri($"amqp://{username}:{password}@{host}:{port}{virtualHost}");
    }
}
