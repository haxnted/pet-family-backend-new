using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Расширения для настройки MassTransit Outbox/Inbox Pattern
/// </summary>
public static class MassTransitOutboxExtensions
{
    /// <summary>
    /// Добавить MassTransit Outbox Pattern для гарантированной доставки событий
    /// </summary>
    /// <typeparam name="TDbContext">Тип DbContext для хранения Outbox сообщений</typeparam>
    public static IServiceCollection AddMassTransitWithOutbox<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureRabbitMq = null)
        where TDbContext : DbContext
    {
        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.AddEntityFrameworkOutbox<TDbContext>(o =>
            {
                o.UsePostgres();
                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSection = configuration.GetSection("RabbitMQ");
                cfg.Host(rabbitMqSection["Host"], h =>
                {
                    h.Username(rabbitMqSection["Username"] ?? "guest");
                    h.Password(rabbitMqSection["Password"] ?? "guest");
                });

                configureRabbitMq?.Invoke(context, cfg);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Добавить MassTransit Inbox Pattern для идемпотентной обработки входящих событий
    /// </summary>
    /// <typeparam name="TDbContext">Тип DbContext для хранения Inbox сообщений</typeparam>
    public static IServiceCollection AddMassTransitWithInbox<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureRabbitMq = null)
        where TDbContext : DbContext
    {
        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.AddEntityFrameworkOutbox<TDbContext>(o =>
            {
                o.UsePostgres();
                o.QueryDelay = TimeSpan.FromSeconds(1);
                o.UseBusOutbox();
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSection = configuration.GetSection("RabbitMQ");
                cfg.Host(rabbitMqSection["Host"], h =>
                {
                    h.Username(rabbitMqSection["Username"] ?? "guest");
                    h.Password(rabbitMqSection["Password"] ?? "guest");
                });

                configureRabbitMq?.Invoke(context, cfg);

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
