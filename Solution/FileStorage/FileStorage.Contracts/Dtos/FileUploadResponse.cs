namespace FileStorage.Contracts.Dtos;

/// <summary>
/// Ответ на загрузку файла
/// </summary>
/// <param name="FileId">Уникальный идентификатор файла</param>
/// <param name="PreSignedUrl">Предподписанный URL для доступа к файлу</param>
/// <param name="FileName">Имя файла</param>
/// <param name="FileSize">Размер файла в байтах</param>
/// <param name="ContentType">MIME-тип содержимого файла</param>
/// <param name="ExpiresAt">Дата и время истечения срока действия предподписанного URL</param>
public record FileUploadResponse(
    Guid FileId,
    string PreSignedUrl,
    string FileName,
    long FileSize,
    string ContentType,
    DateTime ExpiresAt);
