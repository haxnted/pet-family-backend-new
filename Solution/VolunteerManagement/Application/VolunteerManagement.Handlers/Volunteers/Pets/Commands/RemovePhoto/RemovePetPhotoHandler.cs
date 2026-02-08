using FileStorage.Contracts.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Contracts.Events.FileStorage;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.RemovePhoto;

/// <summary>
/// Обработчик команды удаления фотографии питомца.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
/// <param name="publishEndpoint">Endpoint для публикации событий.</param>
/// <param name="fileStorageSettings">Настройки FileStorage.</param>
public class RemovePetPhotoHandler(
    IVolunteerService volunteerService,
    IPublishEndpoint publishEndpoint,
    IOptions<FileStorageSettings> fileStorageSettings)
{
    /// <summary>
    /// Обрабатывает команду удаления фотографии питомца.
    /// </summary>
    /// <param name="command">Команда удаления фотографии.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(RemovePetPhotoCommand command, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(command.VolunteerId, ct);
        var pet = volunteer.GetPetById(PetId.Of(command.PetId));

        pet.RemovePhoto(command.PhotoId);

        var deleteEvent = new PhotoDeleteRequestedEvent
        {
            FileId = command.PhotoId,
            BucketName = fileStorageSettings.Value.BucketName
        };

        await publishEndpoint.Publish(deleteEvent, ct);
    }
}
