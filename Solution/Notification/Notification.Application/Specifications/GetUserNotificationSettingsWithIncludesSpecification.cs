using Ardalis.Specification;
using Notification.Core.Models;

namespace Notification.Application.Specifications;

/// <summary>
/// Спецификация для получения базовых настроек пользователя с вложенными сущностями.
/// </summary>
public sealed class GetUserNotificationSettingsWithIncludesSpecification : Specification<UserNotificationSettings>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public GetUserNotificationSettingsWithIncludesSpecification(Guid userId)
    {
        Query.Include(x => x.EmailSettings)
            .Where(x => x.UserId == userId);
    }
}