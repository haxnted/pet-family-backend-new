using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Infrastructure.Configurations;

namespace PetFamily.SharedKernel.Infrastructure.Configurators;

/// <summary>
/// Базовый класс конфигуратора DbContext.
/// </summary>
public abstract class BaseDbContextConfigurator<TDbContext>(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Имя строки подключения.
    /// </summary>
    protected abstract string ConnectionStringName { get; }

    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<TDbContext> options)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
                               ?? throw new InvalidOperationException($"Строка подключения '{ConnectionStringName}' не найдена.");

        options
            .UseLoggerFactory(loggerFactory)
            .UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                npgsqlOptions.CommandTimeout(60);
                npgsqlOptions.EnableRetryOnFailure();
            })
            .EnableSensitiveDataLogging(); 
    }
}