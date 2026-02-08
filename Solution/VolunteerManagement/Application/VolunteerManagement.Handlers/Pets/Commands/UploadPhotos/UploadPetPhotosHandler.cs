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

        // Получаем волонтёра и животное
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);
        if (volunteer == null)
            throw new InvalidOperationException($"Волонтёр с ID {command.VolunteerId} не найден");

        var pet = volunteer.GetPetById(petId);

        // Создаём Photo ValueObjects
        var photos = command.PhotoIds.Select(Photo.Create).ToList();

        // Добавляем фотографии к питомцу
        pet.AddPhotos(photos);

        // Сохраняем изменения через UpdateAsync
        await volunteerService.UpdateAsync(
            command.VolunteerId,
            volunteer.GeneralDescription.Value,
            volunteer.AgeExperience?.Value,
            volunteer.PhoneNumber?.Value,
            ct);

        logger.LogInformation(
            "Добавлено {Count} фотографий для животного {PetId} волонтёра {VolunteerId}",
            photos.Count, command.PetId, command.VolunteerId);
    }
}
