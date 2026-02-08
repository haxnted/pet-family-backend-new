using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.UpdatePhotos;

/// <summary>
/// Обработчик команды обновления фотографий питомца.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class UpdatePetPhotosHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду обновления фотографий питомца.
    /// </summary>
    /// <param name="command">Команда обновления фотографий.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdatePetPhotosCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);
        var pet = volunteer.GetPetById(PetId.Of(command.PetId));

        var photos = command.PhotoIds
            .Select(Photo.Create)
            .ToList();

        pet.UpdatePhotos(photos);
    }
}
