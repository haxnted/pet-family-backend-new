using Account.Infrastructure.Common;
using Account.Infrastructure.Common.Contexts;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Infrastructure;

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
        services.AddDatabase<AccountDbContext, AccountDbContextConfigurator>();

        services.AddScoped<IMigrator, AccountMigrator>();

        services.AddScoped<IRepository<DomainAccount>, EntityFrameworkRepository<DomainAccount>>();
    }
}
