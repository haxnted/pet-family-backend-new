using Conversation.Domain.Aggregates;
using Conversation.Infrastructure.Common;
using Conversation.Infrastructure.Common.Contexts;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.Infrastructure.Abstractions;

namespace Conversation.Infrastructure;

/// <summary>
/// Класс для настройки инфраструктуры зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Настройка инфраструктуры зависимостей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabase<ConversationDbContext, ConversationDbContextConfigurator>();

        services.AddScoped<IMigrator, ConversationMigrator>();

        services.AddScoped<IRepository<Chat>, EntityFrameworkRepository<Chat>>();
    }
}
