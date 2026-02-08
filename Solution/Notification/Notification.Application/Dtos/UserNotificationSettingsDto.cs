namespace Notification.Application.Dtos;

/// <summary>
/// DTO для базовых настроек уведомлений.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="IsMuted">Мут уведомлений.</param>
/// <param name="CreatedAt">Дата создания.</param>
/// <param name="UpdatedAt">Дата обновления.</param>
/// <param name="EmailSettings">Настройки уведомлений для почты.</param>
public record UserNotificationSettingsDto(
    Guid Id,
    Guid UserId,
    bool IsMuted,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    EmailSettingsDto? EmailSettings);