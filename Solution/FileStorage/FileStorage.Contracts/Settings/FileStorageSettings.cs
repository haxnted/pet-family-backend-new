namespace FileStorage.Contracts.Settings;

/// <summary>
/// Настройки для интеграции с FileStorage микросервисом.
/// </summary>
public class FileStorageSettings
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public const string SectionName = "FileStorageService";

    /// <summary>
    /// Базовый URL FileStorage API.
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:6004";

    /// <summary>
    /// Имя бакета для хранения файлов.
    /// </summary>
    public string BucketName { get; set; } = "petfamily-files";

    /// <summary>
    /// Таймаут HTTP запросов в минутах.
    /// </summary>
    public int TimeoutMinutes { get; set; } = 5;
}
