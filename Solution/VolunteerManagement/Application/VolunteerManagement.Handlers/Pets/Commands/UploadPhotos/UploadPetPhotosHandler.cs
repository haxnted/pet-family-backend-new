using Microsoft.Extensions.Logging;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Pets.Commands.UploadPhotos;

/// <summary>
/// Обработчик команды добавления фотографий к животному.
/// </summary>
public class UploadPetPhotosHandler(
    IVolunteerService volunteerService,
    ILogger<UploadPetPhotosHandler> logger)
{
    /// <summary>
    /// Обрабатывает команду добавления фотографий к животному.
    /// </summary>
    /// <param name="command">Команда добавления фотографий.</param>
    /// <param name="ct">Токен отмены.</param>
    public async Task Handle(UploadPetPhotosCommand command, CancellationToken ct)
    {
        var petId = PetId.Of(command.PetId);

        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);
        if (volunteer == null)
            throw new InvalidOperationException($"Волонтёр с ID {command.VolunteerId} не найден");

        var pet = volunteer.GetPetById(petId);

        var photos = command.PhotoIds.Select(Photo.Create).ToList();

        pet.AddPhotos(photos);

        logger.LogInformation(
            "Добавлено {Count} фотографий для животного {PetId} волонтёра {VolunteerId}",
            photos.Count, command.PetId, command.VolunteerId);
    }
}
