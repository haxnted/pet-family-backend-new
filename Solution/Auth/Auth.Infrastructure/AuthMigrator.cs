using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Infrastructure;

namespace Auth.Infrastructure;

/// <summary>
/// Мигратор базы данных для контекста аутентификации.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
/// <param name="logger">Логгер.</param>
public class AuthMigrator(
    AuthDbContext context,
    ILogger<AuthMigrator> logger)
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

        logger.Log(LogLevel.Information, "Applying auth migrations...");
        await context.Database.MigrateAsync(ct);
        logger.Log(LogLevel.Information, "Migrations auth applied successfully.");
    }
}
