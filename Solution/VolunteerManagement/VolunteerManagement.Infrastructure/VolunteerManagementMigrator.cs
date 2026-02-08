using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Infrastructure;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Infrastructure;

/// <summary>
/// Мигратор базы данных для контекста управления волонтёрами.
/// </summary>
/// <param name="context">Контекст базы данных.</param>
/// <param name="logger">Логгер.</param>
public class VolunteerManagementMigrator(
    VolunteerManagementDbContext context,
    ILogger<VolunteerManagementMigrator> logger)
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

        logger.Log(LogLevel.Information, "Applying volunteers migrations...");
        await context.Database.MigrateAsync(ct);
        logger.Log(LogLevel.Information, "Migrations volunteers applied successfully.");
    }
}