namespace Notification.Core.Models;

/// <summary>
/// Настройки получения уведомлений пользователем.
/// </summary>
public class UserNotificationSettings
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Флаг отключения уведомлений.
    /// </summary>
    public bool IsMuted { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Настройки email-уведомлений.
    /// </summary>
    public EmailSettings? EmailSettings { get; set; }
}