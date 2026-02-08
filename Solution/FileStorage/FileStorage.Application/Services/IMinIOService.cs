namespace FileStorage.Application.Services;

/// <summary>
/// Интерфейс сервиса для работы с MinIO
/// </summary>
public interface IMinIoService
{
    /// <summary>
    /// Проверить существование бакета и создать его при необходимости
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    Task EnsureBucketExistsAsync(CancellationToken ct);

    /// <summary>
    /// Загрузить файл в хранилище
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="stream">Поток данных файла</param>
    /// <param name="fileName">Исходное имя файла</param>
    /// <param name="contentType">MIME-тип файла</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Pre-signed URL для загруженного файла</returns>
    Task<string> UploadFileAsync(
        Guid fileId,
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct);

    /// <summary>
    /// Сгенерировать pre-signed URL для файла
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="expirySeconds">Время жизни ссылки в секундах</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Pre-signed URL</returns>
    Task<string> GetPreSignedUrlAsync(Guid fileId, int expirySeconds, CancellationToken ct);

    /// <summary>
    /// Получить статистику и метаданные объекта
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Статистика объекта или null, если объект не найден</returns>
    Task<ObjectStat?> GetObjectStatAsync(Guid fileId, CancellationToken ct);

    /// <summary>
    /// Удалить файл из хранилища
    /// </summary>
    /// <param name="fileId">Идентификатор файла</param>
    /// <param name="ct">Токен отмены</param>
    Task DeleteFileAsync(Guid fileId, CancellationToken ct);
}