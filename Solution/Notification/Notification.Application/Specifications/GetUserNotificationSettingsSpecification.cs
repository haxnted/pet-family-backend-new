using Ardalis.Specification;
using Notification.Core.Models;

namespace Notification.Application.Specifications;

/// <summary>
/// Спецификация для получения базовых настроек пользователя без вложенных сущностей.
/// </summary>
public sealed class GetUserNotificationSettingsSpecification : Specification<UserNotificationSettings>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public GetUserNotificationSettingsSpecification(Guid userId)
    {
        Query.Where(x => x.UserId == userId);
    }
}