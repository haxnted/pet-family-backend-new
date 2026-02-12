using Conversation.Infrastructure.Common.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Infrastructure;

namespace Conversation.Infrastructure;

/// <summary>
/// Мигратор базы данных для контекста чатов.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
/// <param name="logger">Логгер.</param>
public class ConversationMigrator(
    ConversationDbContext context,
    ILogger<ConversationMigrator> logger)
    : IMigrator
{
    /// <summary>
    /// Выполняет миграцию базы данных.
    /// </summary>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Migrate(CancellationToken ct = default)
    {
        if (!await context.Database.CanConnectAsync(ct))
        {
            await context.Database.EnsureCreatedAsync(ct);
        }

        logger.Log(LogLevel.Information, "Applying conversation migrations...");
        await context.Database.MigrateAsync(ct);
        logger.Log(LogLevel.Information, "Conversation migrations applied successfully.");
    }
}
