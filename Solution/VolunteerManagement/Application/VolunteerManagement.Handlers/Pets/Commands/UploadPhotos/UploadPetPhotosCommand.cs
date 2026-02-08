namespace VolunteerManagement.Handlers.Pets.Commands.UploadPhotos;

/// <summary>
/// Команда для добавления путей к фотографиям животного.
/// </summary>
public record UploadPetPhotosCommand
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор животного.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Список путей к фотографиям в хранилище.
    /// </summary>
    public required List<Guid> PhotoIds { get; init; }
}
