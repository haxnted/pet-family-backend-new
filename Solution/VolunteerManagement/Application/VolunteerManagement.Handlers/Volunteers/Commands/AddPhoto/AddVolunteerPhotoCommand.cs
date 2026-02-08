using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.AddPhoto;

/// <summary>
/// Команда на добавление фотографии волонтёра.
/// </summary>
public sealed class AddVolunteerPhotoCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор файла фотографии в хранилище.
    /// </summary>
    public Guid PhotoId { get; init; }
}
