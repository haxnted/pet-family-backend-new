namespace PetFamily.SharedKernel.Infrastructure.Caching;

/// <summary>
/// Опции конфигурации кеширования.
/// </summary>
public sealed class CachingOptions
{
    /// <summary>
    /// Секция конфигурации в appsettings.json.
    /// </summary>
    public const string SectionName = "Caching";

    /// <summary>
    /// Тип кеша для использования.
    /// </summary>
    public CacheType Type { get; set; } = CacheType.Memory;

    /// <summary>
    /// Время жизни записей в кеше по умолчанию.
    /// </summary>
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Префикс для ключей кеша (используется в распределённом кеше).
    /// Полезно для изоляции данных разных микросервисов.
    /// </summary>
    public string? KeyPrefix { get; set; }

    /// <summary>
    /// Connection string для Redis (используется только при Type = Redis).
    /// </summary>
    public string? RedisConnectionString { get; set; }

    /// <summary>
    /// Имя инстанса Redis (опционально).
    /// </summary>
    public string? RedisInstanceName { get; set; }
}

/// <summary>
/// Тип кеша.
/// </summary>
public enum CacheType
{
    /// <summary>
    /// In-memory кеш (IMemoryCache). Подходит для single-instance приложений.
    /// </summary>
    Memory,

    /// <summary>
    /// Распределённый кеш (IDistributedCache). Для Redis, SQL Server и т.д.
    /// </summary>
    Distributed,

    /// <summary>
    /// Redis кеш напрямую через StackExchange.Redis.
    /// </summary>
    Redis
}
