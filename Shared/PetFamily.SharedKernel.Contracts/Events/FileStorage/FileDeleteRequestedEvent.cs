using PetFamily.SharedKernel.Contracts.Abstractions;

namespace PetFamily.SharedKernel.Contracts.Events.FileStorage;

/// <summary>
/// Событие запроса на удаление файла
/// </summary>
public sealed class FileDeleteRequestedEvent(Guid fileId, string bucketName) : IntegrationEvent
{
    /// <summary>
    /// Идентификатор файла
    /// </summary>
    public Guid FileId { get; } = fileId;

    /// <summary>
    /// Имя bucket в MinIO
    /// </summary>
    public string BucketName { get; } = bucketName;
}
