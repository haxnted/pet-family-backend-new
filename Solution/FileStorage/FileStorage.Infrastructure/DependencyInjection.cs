using FileStorage.Application.Services;
using FileStorage.Infrastructure.MinIo;
using FileStorage.Infrastructure.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.Infrastructure;

/// <summary>
/// Регистрация зависимостей Infrastructure слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости из Infrastructure слоя
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns></returns>
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinIoSettings>(configuration.GetSection(MinIoSettings.SectionName));

        services.AddSingleton<IMinIoService, MinIoService>();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RabbitMQ:Host"] ?? "localhost";
                var port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
                var virtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/";
                var username = configuration["RabbitMQ:Username"] ?? "guest";
                var password = configuration["RabbitMQ:Password"] ?? "guest";

                var rabbitMqUri = new Uri($"amqp://{username}:{password}@{host}:{port}{virtualHost}");
                cfg.Host(rabbitMqUri);

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}