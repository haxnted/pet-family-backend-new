using Notification.Core.Enums;

namespace Notification.Core.Models;

/// <summary>
/// Лог попыток отправки уведомления.
/// </summary>
public class NotificationLog
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор события.
    /// </summary>
    public Guid EventId { get; init; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// JSON-список каналов, по которым предпринималась попытка отправки.
    /// </summary>
    public string ChannelsAttempted { get; init; } = string.Empty;

    /// <summary>
    /// JSON-список каналов, по которым отправка прошла успешно.
    /// </summary>
    public string ChannelsSucceeded { get; init; } = string.Empty;

    /// <summary>
    /// Статус уведомления.
    /// </summary>
    public NotificationStatus Status { get; init; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата и время, после которых запись может быть удалена.
    /// </summary>
    public DateTime ExpiresAt { get; init; }
}