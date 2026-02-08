using PetFamily.SharedKernel.Contracts.Abstractions;

namespace Notification.Contracts.Events;

/// <summary>
/// Integration event for sending notifications to users.
/// </summary>
public class NotificationEvent : IntegrationEvent
{
    /// <summary>
    /// Идентификатор события.
    /// </summary>
    public Guid EventId { get; init; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Тип уведомления.
    /// </summary>
    /// <remarks>
    /// Почта или в телеграмм.
    /// </remarks>
    public required string NotificationType { get; init; }
}