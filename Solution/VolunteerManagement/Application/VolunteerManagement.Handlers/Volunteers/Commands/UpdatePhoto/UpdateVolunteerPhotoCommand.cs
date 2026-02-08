using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.UpdatePhoto;

/// <summary>
/// Команда на обновление фотографии волонтёра.
/// </summary>
public sealed class UpdateVolunteerPhotoCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Путь к новой фотографии в хранилище.
    /// </summary>
    public Guid PhotoId { get; init; }
}
