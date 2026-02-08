namespace FileStorage.Contracts.Dtos;

/// <summary>
/// Информация о файле
/// </summary>
/// <param name="FileId">Уникальный идентификатор файла</param>
/// <param name="FileName">Имя файла</param>
/// <param name="FileSize">Размер файла в байтах</param>
/// <param name="ContentType">MIME-тип содержимого файла</param>
/// <param name="UploadedAt">Дата и время загрузки файла</param>
public record FileInfoDto(
    Guid FileId,
    string FileName,
    long FileSize,
    string ContentType,
    DateTime UploadedAt);
