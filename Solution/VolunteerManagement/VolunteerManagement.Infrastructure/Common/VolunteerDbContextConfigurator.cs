using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PetFamily.SharedKernel.Infrastructure.Configurations;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Infrastructure.Common;

/// <summary>
/// Конфигуратор для <see cref="VolunteerManagementDbContext"/>.
/// </summary>
public sealed class VolunteerDbContextConfigurator(IConfiguration configuration, ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<VolunteerManagementDbContext>
{
    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<VolunteerManagementDbContext> options)
    {
        var connectionString = configuration.GetConnectionString("VolunteerDbContext")
                               ?? throw new InvalidOperationException("Строка подключения 'VolunteerDbContext' не найдена.");

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
