using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.UpdatePhoto;

/// <summary>
/// Обработчик команды обновления фотографии волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class UpdateVolunteerPhotoHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду обновления фотографии волонтёра.
    /// </summary>
    /// <param name="command">Команда обновления фотографии.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdateVolunteerPhotoCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);

        var photo = Photo.Create(command.PhotoId);
        volunteer.UpdatePhoto(photo);
    }
}
