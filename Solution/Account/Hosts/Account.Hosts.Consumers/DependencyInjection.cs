using Account.Handlers.Commands.Create;
using Account.Hosts.Consumers.Consumers;
using Account.Hosts.DI;
using Account.Infrastructure.Common.Contexts;
using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace Account.Hosts.Consumers;

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

        services.AddScoped<CreateAccountHandler>();

        services.AddMassTransitWithInbox<AccountDbContext>(
            configuration,
            configureBus: (cfg) => cfg.AddConsumer<UserCreatedEventConsumer>(),
            configureRabbitMq: (context, cfg) =>
            {
                cfg.Message<UserCreatedEvent>(e => e.SetEntityName("auth-events"));

                cfg.ReceiveEndpoint("account-user-events", e =>
                {
                    e.Bind("auth-events");
                    e.ConfigureConsumer<UserCreatedEventConsumer>(context);
                });
            });
    }
}
