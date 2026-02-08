namespace Notification.Core.Enums;

/// <summary>
/// Статус отправки уведомления.
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// Отправка уведомления ожидает обработки.
    /// </summary>
    Pending,

    /// <summary>
    /// Уведомление успешно отправлено хотя бы по одному каналу.
    /// </summary>
    Sent,

    /// <summary>
    /// Все попытки отправки уведомления завершились неудачей.
    /// </summary>
    Failed
}