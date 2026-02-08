using FileStorage.Contracts.Dtos;

namespace FileStorage.Application.Services;

/// <summary>
/// Сервис для работы с файлами
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Загрузить файл в хранилище и вернуть информацию о загруженном файле
    /// </summary>
    /// <param name="fileStream">Поток данных файла</param>
    /// <param name="fileName">Исходное имя файла</param>
    /// <param name="contentType">MIME-тип файла</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Информация о загруженном файле</returns>
    Task<FileUploadResponse> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken ct);

    /// <summary>
    /// Сгенерировать pre-signed URL для доступа к файлу
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="expirySeconds">Время жизни ссылки в секундах</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Pre-signed URL для доступа к файлу</returns>
    Task<string> GetPreSignedUrlAsync(Guid fileId, int expirySeconds, CancellationToken ct);

    /// <summary>
    /// Получить метаданные файла
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Информация о файле или null, если файл не найден</returns>
    Task<FileInfoDto?> GetFileInfoAsync(Guid fileId, CancellationToken ct);

    /// <summary>
    /// Удалить файл из хранилища
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="ct">Токен отмены</param>
    Task DeleteAsync(Guid fileId, CancellationToken ct);
}