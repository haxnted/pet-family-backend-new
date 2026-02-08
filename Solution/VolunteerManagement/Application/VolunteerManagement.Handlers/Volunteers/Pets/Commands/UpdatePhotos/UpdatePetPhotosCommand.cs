using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.UpdatePhotos;

/// <summary>
/// Команда на обновление фотографий питомца.
/// </summary>
public sealed class UpdatePetPhotosCommand : Command
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
    /// Новые пути к фотографиям в хранилище.
    /// </summary>
    public required List<Guid> PhotoIds { get; init; }
}
