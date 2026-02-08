namespace Notification.Application.Dtos;

/// <summary>
/// Настройки уведомлений для почты.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="Email">Почта.</param>
/// <param name="IsEnabled">Флаг, указывающий включены ли обновления</param>
public record EmailSettingsDto(Guid Id, string Email, bool IsEnabled);