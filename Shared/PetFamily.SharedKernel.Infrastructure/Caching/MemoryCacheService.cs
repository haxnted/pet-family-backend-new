using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace PetFamily.SharedKernel.Infrastructure.Caching;

/// <summary>
/// Реализация кеш-сервиса на основе IMemoryCache.
/// Подходит для single-instance приложений и разработки.
/// </summary>
public sealed class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly CachingOptions _options;
    private readonly ConcurrentDictionary<string, byte> _keys = new();
    private readonly ConcurrentDictionary<string, object> _getOrSetTasks = new();

    public MemoryCacheService(IMemoryCache cache, IOptions<CachingOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string key, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var result = _cache.TryGetValue(key, out T? value) ? value : default;
        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, CancellationToken ct, TimeSpan? expiration = null)
    {
        ct.ThrowIfCancellationRequested();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? _options.DefaultExpiration
        };

        cacheOptions.RegisterPostEvictionCallback(OnEvicted, _keys);

        _cache.Set(key, value, cacheOptions);
        _keys.TryAdd(key, 0);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        _cache.Remove(key);
        _keys.TryRemove(key, out _);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T?>> factory,
        CancellationToken ct,
        TimeSpan? expiration = null)
    {
        ct.ThrowIfCancellationRequested();

        var cached = await GetAsync<T>(key, ct);
        if (cached != null)
        {
            return cached;
        }

        var task = (Task<T?>)_getOrSetTasks.GetOrAdd(
            key,
            k => (object)RunFactoryAndCacheAsync(k, factory, ct, expiration));

        try
        {
            return await task;
        }
        finally
        {
            _getOrSetTasks.TryRemove(key, out _);
        }
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(string key, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return Task.FromResult(_cache.TryGetValue(key, out _));
    }

    private async Task<T?> RunFactoryAndCacheAsync<T>(
        string key,
        Func<CancellationToken, Task<T?>> factory,
        CancellationToken ct,
        TimeSpan? expiration)
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

    private static void OnEvicted(object key, object? value, EvictionReason reason, object? state)
    {
        if (state is ConcurrentDictionary<string, byte> keys)
        {
            keys.TryRemove(key.ToString()!, out _);
        }
    }
}
