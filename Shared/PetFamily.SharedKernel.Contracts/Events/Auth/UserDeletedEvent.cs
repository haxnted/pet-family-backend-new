using PetFamily.SharedKernel.Contracts.Abstractions;

namespace PetFamily.SharedKernel.Contracts.Events.Auth;

/// <summary>
/// Событие удаления пользователя.
/// </summary>
public sealed class UserDeletedEvent(Guid userId) : IntegrationEvent
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; } = userId;
}