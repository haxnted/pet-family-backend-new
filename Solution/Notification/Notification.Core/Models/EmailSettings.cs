namespace Notification.Core.Models;

/// <summary>
/// Настройки для почты.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор базовых настроек уведомлений.
    /// </summary>
    public Guid UserNotificationSettingsId { get; init; }

    /// <summary>
    /// Почта.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Флаг, указывающий включены ли обновления
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Ссылка на базовые настройки уведомлений.
    /// </summary>
    public UserNotificationSettings UserNotificationSettings { get; init; } = null!;
}