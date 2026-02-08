namespace PetFamily.SharedKernel.Infrastructure.Caching;

/// <summary>
/// Абстракция сервиса кеширования для использования в микросервисах.
/// Поддерживает как in-memory кеш, так и распределённый (Redis).
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Получает значение из кеша по ключу.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кеша.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Значение из кеша или null, если не найдено.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken ct);

    /// <summary>
    /// Сохраняет значение в кеш.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кеша.</param>
    /// <param name="value">Значение для сохранения.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <param name="expiration">Время жизни записи. Если null, используется значение по умолчанию.</param>
    Task SetAsync<T>(string key, T value, CancellationToken ct, TimeSpan? expiration = null);

    /// <summary>
    /// Удаляет значение из кеша по ключу.
    /// </summary>
    /// <param name="key">Ключ кеша.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RemoveAsync(string key, CancellationToken ct);

    /// <summary>
    /// Получает значение из кеша или вычисляет и сохраняет его, если отсутствует.
    /// </summary>
    /// <typeparam name="T">Тип значения.</typeparam>
    /// <param name="key">Ключ кеша.</param>
    /// <param name="factory">Фабрика для создания значения, если его нет в кеше.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <param name="expiration">Время жизни записи. Если null, используется значение по умолчанию.</param>
    /// <returns>Значение из кеша или вычисленное значение.</returns>
    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T?>> factory,
        CancellationToken ct,
        TimeSpan? expiration = null);

    /// <summary>
    /// Проверяет наличие ключа в кеше.
    /// </summary>
    /// <param name="key">Ключ кеша.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>True, если ключ существует.</returns>
    Task<bool> ExistsAsync(string key, CancellationToken ct);
}