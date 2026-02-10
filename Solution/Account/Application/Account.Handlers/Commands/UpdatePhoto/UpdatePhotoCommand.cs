using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Handlers.Commands.UpdatePhoto;

/// <summary>
/// Команда на обновление фотографии профиля.
/// </summary>
public sealed class UpdatePhotoCommand : Command
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор фотографии (null для удаления).
    /// </summary>
    public Guid? PhotoId { get; init; }
}
