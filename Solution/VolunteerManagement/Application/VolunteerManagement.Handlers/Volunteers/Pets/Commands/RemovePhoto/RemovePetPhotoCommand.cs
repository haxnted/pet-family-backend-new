using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.RemovePhoto;

/// <summary>
/// Команда на удаление фотографии питомца.
/// </summary>
public sealed class RemovePetPhotoCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public Guid PetId { get; init; }

    /// <summary>
    /// Путь к фотографии для удаления.
    /// </summary>
    public Guid PhotoId { get; init; }
}
