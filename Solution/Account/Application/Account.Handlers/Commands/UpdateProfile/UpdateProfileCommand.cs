using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Handlers.Commands.UpdateProfile;

/// <summary>
/// Команда на обновление профильных данных аккаунта.
/// </summary>
public sealed class UpdateProfileCommand : Command
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Опыт (в годах).
    /// </summary>
    public int? AgeExperience { get; init; }

    /// <summary>
    /// Описание профиля.
    /// </summary>
    public string? Description { get; init; }
}
