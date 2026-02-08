using PetFamily.SharedKernel.Contracts.Abstractions;

namespace PetFamily.SharedKernel.Contracts.Events.FileStorage;

/// <summary>
/// Событие запроса на удаление фотографии
/// </summary>
public sealed class PhotoDeleteRequestedEvent : IntegrationEvent
{
    /// <summary>
    /// Идентификатор файла фотографии
    /// </summary>
    public Guid FileId { get; init; }

    /// <summary>
    /// Имя bucket в MinIO
    /// </summary>
    public string BucketName { get; init; } = string.Empty;
}
