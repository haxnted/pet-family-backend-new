using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace PetFamily.SharedKernel.Infrastructure.Caching;

/// <summary>
/// Реализация кеш-сервиса на основе IDistributedCache.
/// Подходит для распределённых систем (Redis, SQL Server, NCache).
/// </summary>
public sealed class DistributedCacheService(
    IDistributedCache cache,
    IOptions<CachingOptions> options) : ICacheService
{
    private static readonly JsonSerializerOptions DefaultJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private readonly CachingOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        var prefixedKey = GetPrefixedKey(key);
        var bytes = await cache.GetAsync(prefixedKey, ct);

        if (bytes == null || bytes.Length == 0)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(bytes.AsSpan(), DefaultJsonOptions);
    }

    /// <inheritdoc />
    public async Task SetAsync<T>(
        string key,
        T value,
        CancellationToken ct,
        TimeSpan? expiration = null
    )
    {
        var prefixedKey = GetPrefixedKey(key);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, DefaultJsonOptions);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? _options.DefaultExpiration
        };

        await cache.SetAsync(prefixedKey, bytes, cacheOptions, ct);
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key, CancellationToken ct)
    {
        var prefixedKey = GetPrefixedKey(key);
        await cache.RemoveAsync(prefixedKey, ct);
    }

    /// <inheritdoc />
    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T?>> factory,
        CancellationToken ct,
        TimeSpan? expiration = null)
    {
        var cached = await GetAsync<T>(key, ct);
        if (cached != null)
        {
            return cached;
        }

        var value = await factory(ct);
        if (value != null)
        {
            await SetAsync(key, value, ct, expiration);
        }

        return value;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string key, CancellationToken ct)
    {
        var prefixedKey = GetPrefixedKey(key);
        var bytes = await cache.GetAsync(prefixedKey, ct);
        return bytes != null && bytes.Length > 0;
    }

    private string GetPrefixedKey(string key)
    {
        return string.IsNullOrEmpty(_options.KeyPrefix)
            ? key
            : $"{_options.KeyPrefix}:{key}";
    }
}