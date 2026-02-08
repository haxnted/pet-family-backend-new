using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using PetFamily.SharedKernel.WebApi.Extensions;
using VolunteerManagement.Handlers.Volunteers.Commands.Add;
using VolunteerManagement.Hosts.Consumers.Consumers;
using VolunteerManagement.Hosts.DI;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Hosts.Consumers;

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

        services.AddScoped<AddVolunteerHandler>();

        services.AddMassTransitWithInbox<VolunteerManagementDbContext>(
            configuration,
            configureBus: (cfg) => cfg.AddConsumer<UserCreatedEventConsumer>(),
            configureRabbitMq: (context, cfg) =>
            {
                cfg.Message<UserCreatedEvent>(e => e.SetEntityName("auth-events"));

                cfg.ReceiveEndpoint("volunteer-management-user-events", e =>
                {
                    e.Bind("auth-events");
                    e.ConfigureConsumer<UserCreatedEventConsumer>(context);
                });
            });
    }
}