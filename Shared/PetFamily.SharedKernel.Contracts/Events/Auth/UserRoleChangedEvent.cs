using PetFamily.SharedKernel.Contracts.Abstractions;

namespace PetFamily.SharedKernel.Contracts.Events.Auth;

/// <summary>
/// Событие изменения роли пользователя.
/// </summary>
public sealed class UserRoleChangedEvent(Guid userId, string oldRole, string newRole) : IntegrationEvent
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// Старая роль пользователя.
    /// </summary>
    public string OldRole { get; } = oldRole;

    /// <summary>
    /// Новая роль пользователя.
    /// </summary>
    public string NewRole { get; } = newRole;
}