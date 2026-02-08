namespace FileStorage.Infrastructure.Settings;

/// <summary>
/// Настройки подключения к MinIO
/// </summary>
public class MinIoSettings
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public const string SectionName = "MinIO";

    /// <summary>
    /// Адрес сервера MinIO (например, localhost:9000)
    /// </summary>
    public required string Endpoint { get; init; }

    /// <summary>
    /// Access Key для MinIO
    /// </summary>
    public required string AccessKey { get; init; }

    /// <summary>
    /// Secret Key для MinIO
    /// </summary>
    public required string SecretKey { get; init; }

    /// <summary>
    /// Имя bucket для хранения файлов
    /// </summary>
    public required string BucketName { get; init; }

    /// <summary>
    /// Использовать SSL/TLS
    /// </summary>
    public bool UseSSL { get; init; }
}