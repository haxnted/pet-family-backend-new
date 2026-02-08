using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Data;
using PetFamily.SharedKernel.Infrastructure;

namespace Notification.Infrastructure;

/// <summary>
/// Мигратор базы данных для контекста уведомлений.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
/// <param name="logger">Логгер.</param>
public class NotificationMigrator(
    NotificationDbContext context,
    ILogger<NotificationMigrator> logger)
    : IMigrator
{
    /// <summary>
    /// Выполняет миграцию базы данных.
    /// </summary>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Migrate(CancellationToken ct)
    {
        if (!await context.Database.CanConnectAsync(ct))
        {
            await context.Database.EnsureCreatedAsync(ct);
        }

        logger.Log(LogLevel.Information, "Applying notification migrations...");
        await context.Database.MigrateAsync(ct);
        logger.Log(LogLevel.Information, "Migrations notification applied successfully.");
    }
}
