using Notification.Application.Dtos;

namespace Notification.Application.Services;

/// <summary>
/// Сервис для управления настройками уведомлений пользователей.
/// </summary>
public interface INotificationSettingsService
{
    /// <summary>
    /// Создаёт конфигурацию уведомлений для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task CreateConfigurationAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Получает настройки уведомлений по идентификатору пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<UserNotificationSettingsDto> GetSettingsByIdAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Обновляет настройки уведомлений пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="isEmailNotifyEnabled">Флаг, указывающий, включены ли email-уведомления.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateAsync(Guid userId, bool isEmailNotifyEnabled, CancellationToken ct);

    /// <summary>
    /// Отключает все уведомления для пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task DisableNotifyAsync(Guid userId, CancellationToken ct);
}