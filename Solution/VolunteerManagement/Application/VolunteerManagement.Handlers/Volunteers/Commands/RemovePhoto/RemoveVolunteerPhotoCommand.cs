using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.RemovePhoto;

/// <summary>
/// Команда на удаление фотографии волонтёра.
/// </summary>
public sealed class RemoveVolunteerPhotoCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}
