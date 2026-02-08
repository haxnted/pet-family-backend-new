using FileStorage.Contracts.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Contracts.Events.FileStorage;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.RemovePhoto;

/// <summary>
/// Обработчик команды удаления фотографии волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
/// <param name="publishEndpoint">Endpoint для публикации событий.</param>
/// <param name="fileStorageSettings">Настройки FileStorage.</param>
public class RemoveVolunteerPhotoHandler(
    IVolunteerService volunteerService,
    IPublishEndpoint publishEndpoint,
    IOptions<FileStorageSettings> fileStorageSettings)
{
    /// <summary>
    /// Обрабатывает команду удаления фотографии волонтёра.
    /// </summary>
    /// <param name="command">Команда удаления фотографии.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(RemoveVolunteerPhotoCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);

        var photoId = volunteer.Photo?.Value;

        volunteer.RemovePhoto();

        if (photoId.HasValue)
        {
            var deleteEvent = new PhotoDeleteRequestedEvent
            {
                FileId = photoId.Value,
                BucketName = fileStorageSettings.Value.BucketName
            };

            await publishEndpoint.Publish(deleteEvent, ct);
        }
    }
}
