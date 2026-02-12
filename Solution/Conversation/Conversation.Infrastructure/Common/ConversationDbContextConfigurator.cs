using Conversation.Infrastructure.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PetFamily.SharedKernel.Infrastructure.Configurations;

namespace Conversation.Infrastructure.Common;

/// <summary>
/// Конфигуратор для <see cref="ConversationDbContext"/>.
/// </summary>
public sealed class ConversationDbContextConfigurator(IConfiguration configuration, ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<ConversationDbContext>
{
    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<ConversationDbContext> options)
    {
        var connectionString = configuration.GetConnectionString("ConversationDbContext")
                               ?? throw new InvalidOperationException("Строка подключения 'ConversationDbContext' не найдена.");

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
