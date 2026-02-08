using MassTransit;
using Notification.Hosts.DI;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Messaging;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace Notification.Hosts.Consumers;

/// <summary>
/// Класс для настройки зависимостей для worker сервиса.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Настройка зависимостей worker сервиса.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostDependencies(configuration);

        services.AddMassTransitWithInbox<NotificationDbContext>(
            configuration,
            configureBus: cfg =>
            {
                cfg.AddConsumer<NotificationEventConsumer>();
                cfg.AddConsumer<UserCreatedEventConsumer>();
            },
            configureRabbitMq: (context, cfg) =>
            {
                cfg.Message<UserCreatedEvent>(e => e.SetEntityName("auth-events"));

                cfg.ReceiveEndpoint("notification-events", e =>
                {
                    e.ConfigureConsumer<NotificationEventConsumer>(context);
                });

                cfg.ReceiveEndpoint("notification-user-created", e =>
                {
                    e.Bind("auth-events");
                    e.ConfigureConsumer<UserCreatedEventConsumer>(context);
                });
            });
    }
}
