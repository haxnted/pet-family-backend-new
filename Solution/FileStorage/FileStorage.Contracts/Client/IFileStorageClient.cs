using FileStorage.Contracts.Dtos;

namespace FileStorage.Contracts.Client;

/// <summary>
/// Клиент для взаимодействия с FileStorage микросервисом.
/// </summary>
public interface IFileStorageClient
{
    /// <summary>
    /// Загружает файл в FileStorage и возвращает метаданные с предподписанным URL.
    /// </summary>
    /// <param name="stream">Поток данных файла.</param>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="contentType">MIME-тип содержимого файла.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Ответ с идентификатором файла и предподписанным URL.</returns>
    Task<FileUploadResponse> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken ct);
}