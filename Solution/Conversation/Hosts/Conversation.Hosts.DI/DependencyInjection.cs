using Conversation.Handlers.Commands.CreateChat;
using Conversation.Infrastructure;
using Conversation.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure.Caching;
using PetFamily.SharedKernel.Infrastructure.Options;
using Wolverine;

namespace Conversation.Hosts.DI;

/// <summary>
/// Содержит все внедренные зависимости для Host приложений.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости для Host приложений.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddHostDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));

        services.AddApplication();
        services.AddWolverine(opts => { opts.Discovery.IncludeAssembly(typeof(CreateChatHandler).Assembly); });
        services.AddInfrastructure();

        services.AddCaching(configuration);

        return services;
    }
}
