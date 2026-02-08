using FileStorage.Application.Services;
using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.FileStorage;

namespace FileStorage.Consumers.Consumers;

/// <summary>
/// Consumer для обработки событий удаления файлов.
/// Выполняет физическое удаление файла из MinIO.
/// </summary>
public class FileDeleteRequestedConsumer(
    IMinIoService minioService,
    ILogger<FileDeleteRequestedConsumer> logger)
    : IConsumer<FileDeleteRequestedEvent>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<FileDeleteRequestedEvent> context)
    {
        var deleteEvent = context.Message;

        logger.LogInformation(
            "Получено событие FileDeleteRequestedEvent для файла {FileId} в bucket {BucketName}",
            deleteEvent.FileId,
            deleteEvent.BucketName);

        try
        {
            await minioService.DeleteFileAsync(
                deleteEvent.FileId,
                context.CancellationToken);

            logger.LogInformation(
                "Файл {FileId} успешно удален из bucket {BucketName}",
                deleteEvent.FileId,
                deleteEvent.BucketName);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка при удалении файла {FileId} из bucket {BucketName}",
                deleteEvent.FileId,
                deleteEvent.BucketName);

            throw;
        }
    }
}
