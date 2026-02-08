using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.AddPhoto;

/// <summary>
/// Обработчик команды добавления фотографии волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class AddVolunteerPhotoHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду добавления фотографии волонтёра.
    /// </summary>
    /// <param name="command">Команда добавления фотографии.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(AddVolunteerPhotoCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);

        var photo = Photo.Create(command.PhotoId);
        volunteer.AddPhoto(photo);

        // Сохранение происходит через Unit of Work в сервисе
    }
}
