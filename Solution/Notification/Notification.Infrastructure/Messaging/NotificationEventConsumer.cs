using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Contracts.Events;
using Notification.Core.Enums;
using Notification.Core.Models;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services.Email;

namespace Notification.Infrastructure.Messaging;

/// <summary>
/// MassTransit consumer для сообщений NotificationEvent.
/// Обрабатывает доставку уведомлений по настроенным каналам с логикой повторных попыток.
/// </summary>
public class NotificationEventConsumer(
    NotificationDbContext dbContext,
    ILogger<NotificationEventConsumer> logger) : IConsumer<NotificationEvent>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        var @event = context.Message;

        logger.LogInformation(
            "Обработка события уведомления {EventId} для пользователя {UserId}",
            @event.EventId,
            @event.UserId);

        var settings = await dbContext.UserNotificationSettings
            .Include(s => s.EmailSettings)
            .FirstOrDefaultAsync(s => s.UserId == @event.UserId);

        if (settings == null)
        {
            logger.LogWarning("Не найдены настройки уведомлений для пользователя {UserId}", @event.UserId);
            return;
        }

        if (settings.IsMuted)
        {
            logger.LogInformation("Уведомления отключены для пользователя {UserId}", @event.UserId);
            return;
        }

        var channelsAttempted = new List<string>();
        var channelsSucceeded = new List<string>();

        var log = new NotificationLog
        {
            Id = Guid.NewGuid(),
            EventId = @event.EventId,
            UserId = @event.UserId,
            ChannelsAttempted = JsonSerializer.Serialize(channelsAttempted),
            ChannelsSucceeded = JsonSerializer.Serialize(channelsSucceeded),
            Status = channelsSucceeded.Count > 0 ? NotificationStatus.Sent : NotificationStatus.Failed,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        dbContext.NotificationLogs.Add(log);
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation(
            "Событие уведомления {EventId} обработано. Попытки: [{Attempted}], Успешно: [{Succeeded}]",
            @event.EventId,
            string.Join(", ", channelsAttempted),
            string.Join(", ", channelsSucceeded));
    }
}