using Conversation.Hosts.Consumers.Consumers;
using Conversation.Hosts.DI;
using Conversation.Infrastructure.Common.Contexts;
using MassTransit;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace Conversation.Hosts.Consumers;

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

        services.AddMassTransitWithInbox<ConversationDbContext>(
            configuration,
            configureBus: (cfg) => cfg.AddConsumer<CreateAdoptionChatConsumer>(),
            configureRabbitMq: (context, cfg) =>
            {
                cfg.ReceiveEndpoint("conversation-create-adoption-chat", e =>
                {
                    e.ConfigureConsumer<CreateAdoptionChatConsumer>(context);
                });
            });
    }
}
