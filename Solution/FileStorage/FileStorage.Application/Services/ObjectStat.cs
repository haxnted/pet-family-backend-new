namespace FileStorage.Application.Services;

/// <summary>
/// Статистика и метаданные объекта хранилища
/// </summary>
public class ObjectStat
{
    /// <summary>
    /// Размер объекта в байтах
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// MIME-тип содержимого объекта
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Метаданные объекта
    /// </summary>
    public Dictionary<string, string> MetaData { get; set; } = new();
}