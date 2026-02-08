using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.AddPhotos;

/// <summary>
/// Команда на добавление фотографий питомца.
/// </summary>
public sealed class AddPetPhotosCommand : Command
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
    /// Пути к фотографиям в хранилище.
    /// </summary>
    public List<Guid> PhotoIds { get; init; } = null!;
}
