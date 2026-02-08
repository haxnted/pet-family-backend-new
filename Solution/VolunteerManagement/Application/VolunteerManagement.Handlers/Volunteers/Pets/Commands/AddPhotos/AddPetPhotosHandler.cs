using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.AddPhotos;

/// <summary>
/// Обработчик команды добавления фотографий питомца.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class AddPetPhotosHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду добавления фотографий питомца.
    /// </summary>
    /// <param name="command">Команда добавления фотографий.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(AddPetPhotosCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);
        var pet = volunteer.GetPetById(PetId.Of(command.PetId));

        var photos = command.PhotoIds
            .Select(Photo.Create)
            .ToList();

        pet.AddPhotos(photos);
    }
}
