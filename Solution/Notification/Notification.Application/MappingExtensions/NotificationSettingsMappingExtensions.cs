using Notification.Application.Dtos;
using Notification.Core.Models;

namespace Notification.Application.MappingExtensions;

/// <summary>
/// Extension-методы для преобразования доменных моделей настроек уведомлений в DTO.
/// </summary>
public static class NotificationSettingsMappingExtensions
{
    /// <summary>
    /// Преобразует доменную модель пользовательских настроек уведомлений
    /// в DTO <see cref="UserNotificationSettingsDto"/>.
    /// </summary>
    /// <param name="settings"> Доменная сущность пользовательских настроек уведомлений.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="settings"/> равен <c>null</c>.
    /// </exception>
    public static UserNotificationSettingsDto ToDto(this UserNotificationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new UserNotificationSettingsDto(
            settings.Id,
            settings.UserId,
            settings.IsMuted,
            settings.CreatedAt,
            settings.UpdatedAt,
            settings.EmailSettings?.ToDto()
        );
    }

    /// <summary>
    /// Преобразует доменную модель email-настроек в DTO <see cref="EmailSettingsDto"/>.
    /// </summary>
    /// <param name="settings"> Доменная сущность настроек email-уведомлений.</param>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="settings"/> равен <c>null</c>.
    /// </exception>
    private static EmailSettingsDto ToDto(this EmailSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return new EmailSettingsDto(
            settings.Id,
            settings.Email,
            settings.IsEnabled
        );
    }
}