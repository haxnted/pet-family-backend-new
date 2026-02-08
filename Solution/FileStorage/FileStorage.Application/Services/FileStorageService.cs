using FileStorage.Contracts.Dtos;
using Microsoft.Extensions.Logging;

namespace FileStorage.Application.Services;

/// <inheritdoc/>
public class FileStorageService(
    IMinIoService minIoService,
    ILogger<FileStorageService> logger) : IFileStorageService
{
    /// <inheritdoc/>
    public async Task<FileUploadResponse> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken ct)
    {
        var fileId = Guid.NewGuid();

        logger.LogInformation("Начало загрузки файла {FileName} с ID {FileId}", fileName, fileId);

        await minIoService.UploadFileAsync(
            fileId,
            fileStream,
            fileName,
            contentType,
            ct);

        var preSignedUrl = await minIoService.GetPreSignedUrlAsync(fileId, 3600, ct);

        logger.LogInformation("Файл {FileId} успешно загружен", fileId);

        return new FileUploadResponse(
            fileId,
            preSignedUrl,
            fileName,
            fileStream.Length,
            contentType,
            DateTime.UtcNow.AddHours(1));
    }

    /// <inheritdoc/>
    public async Task<string> GetPreSignedUrlAsync(
        Guid fileId,
        int expirySeconds,
        CancellationToken ct)
    {
        logger.LogInformation("Генерация pre-signed URL для файла {FileId}", fileId);

        return await minIoService.GetPreSignedUrlAsync(fileId, expirySeconds, ct);
    }

    /// <inheritdoc/>
    public async Task<FileInfoDto?> GetFileInfoAsync(Guid fileId, CancellationToken ct)
    {
        logger.LogInformation(
            "Получение метаданных файла {FileId}",
            fileId);

        var objectStat = await minIoService.GetObjectStatAsync(
            fileId,
            ct);

        if (objectStat == null)
        {
            logger.LogWarning(
                "Файл {FileId} не найден",
                fileId);

            return null;
        }

        var fileName = objectStat.MetaData.TryGetValue(
            "original-filename",
            out var name)
            ? name
            : $"{fileId}.file";

        var uploadedAt = objectStat.MetaData.TryGetValue(
            "uploaded-at",
            out var dateStr)
            ? DateTime.Parse(dateStr)
            : DateTime.UtcNow;

        return new FileInfoDto(
            fileId,
            fileName,
            objectStat.Size,
            objectStat.ContentType,
            uploadedAt);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid fileId, CancellationToken ct)
    {
        logger.LogInformation("Удаление файла {FileId}", fileId);

        await minIoService.DeleteFileAsync(fileId, ct);

        logger.LogInformation("Файл {FileId} успешно удалён", fileId);
    }
}
