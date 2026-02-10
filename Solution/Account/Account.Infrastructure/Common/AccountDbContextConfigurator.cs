using Account.Infrastructure.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PetFamily.SharedKernel.Infrastructure.Configurations;

namespace Account.Infrastructure.Common;

/// <summary>
/// Конфигуратор для <see cref="AccountDbContext"/>.
/// </summary>
public sealed class AccountDbContextConfigurator(IConfiguration configuration, ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<AccountDbContext>
{
    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<AccountDbContext> options)
    {
        var connectionString = configuration.GetConnectionString("AccountDbContext")
                               ?? throw new InvalidOperationException("Строка подключения 'AccountDbContext' не найдена.");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        options
            .UseLoggerFactory(loggerFactory)
            .UseNpgsql(dataSource, npgsqlOptions =>
            {
                npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                npgsqlOptions.CommandTimeout(60);
                npgsqlOptions.EnableRetryOnFailure();
            })
            .EnableSensitiveDataLogging();
    }
}
