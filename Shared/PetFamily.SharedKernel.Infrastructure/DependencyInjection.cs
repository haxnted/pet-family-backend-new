using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.Infrastructure.Configurations;
using PetFamily.SharedKernel.Infrastructure.Transactions;

namespace PetFamily.SharedKernel.Infrastructure;

/// <summary>
/// Конфигурация зависимостей базы данных.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавление зависимостей базы данных.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddDatabase<TDbContext, TDbContextConfigurator>(this IServiceCollection services)
        where TDbContext : DbContext
        where TDbContextConfigurator : class, IDbContextOptionsConfigurator<TDbContext>
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<TDbContext>(Configure<TDbContext>);

        services
            .AddSingleton<IDbContextOptionsConfigurator<TDbContext>, TDbContextConfigurator>()
            .AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>())
            .AddScoped<ITransactionalExecutor, TransactionalExecutor<TDbContext>>();
    }

    private static void Configure<TDbContext>(IServiceProvider sp, DbContextOptionsBuilder dbOptions)
        where TDbContext : DbContext
    {
        var configurator = sp.GetRequiredService<IDbContextOptionsConfigurator<TDbContext>>();
        configurator.Configure((DbContextOptionsBuilder<TDbContext>)dbOptions);
    }
}