using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Infrastructure.Data;

namespace Notification.Infrastructure.BackgroundJobs;

/// <summary>
/// Фоновая служба, которая периодически очищает устаревшие записи уведомлений.
/// </summary>
public class NotificationLogCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationLogCleanupService> logger) : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(1);

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Служба очистки логов уведомлений запущена");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(CleanupInterval, stoppingToken);
                await CleanupExpiredLogsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при очистке логов уведомлений");
            }
        }

        logger.LogInformation("Служба очистки логов уведомлений остановлена");
    }

    /// <summary>
    /// Очистить устаревшие записи уведомлений.
    /// </summary>
    /// <param name="ct"></param>
    private async Task CleanupExpiredLogsAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

        var deleted = await dbContext.NotificationLogs
            .Where(l => l.ExpiresAt < DateTime.UtcNow)
            .ExecuteDeleteAsync(ct);

        if (deleted > 0)
        {
            logger.LogInformation("Удалено {Count} устаревших записей уведомлений", deleted);
        }
    }
}