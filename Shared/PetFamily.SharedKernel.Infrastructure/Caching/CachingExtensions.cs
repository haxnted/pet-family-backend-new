using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.Infrastructure.Caching;

/// <summary>
/// Extension методы для регистрации сервисов кеширования.
/// </summary>
public static class CachingExtensions
{
    /// <summary>
    /// Добавляет сервис кеширования на основе конфигурации.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов для chaining.</returns>
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = new CachingOptions();
        configuration.GetSection(CachingOptions.SectionName).Bind(options);

        return services.AddCaching(opt =>
        {
            opt.Type = options.Type;
            opt.DefaultExpiration = options.DefaultExpiration;
            opt.KeyPrefix = options.KeyPrefix;
            opt.RedisConnectionString = options.RedisConnectionString;
            opt.RedisInstanceName = options.RedisInstanceName;
        });
    }

    /// <summary>
    /// Добавляет сервис кеширования с ручной конфигурацией.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configure">Делегат для настройки опций.</param>
    /// <returns>Коллекция сервисов для chaining.</returns>
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        Action<CachingOptions> configure)
    {
        var options = new CachingOptions();
        configure(options);

        services.Configure(configure);

        return options.Type switch
        {
            CacheType.Memory => services.AddMemoryCaching(),
            CacheType.Distributed => services.AddDistributedCaching(options),
            CacheType.Redis => services.AddRedisCaching(options),
            _ => services.AddMemoryCaching()
        };
    }

    /// <summary>
    /// Добавляет in-memory кеширование.
    /// Подходит для single-instance приложений и разработки.
    /// </summary>
    public static IServiceCollection AddMemoryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();
        return services;
    }

    /// <summary>
    /// Добавляет распределённое кеширование через IDistributedCache.
    /// </summary>
    public static IServiceCollection AddDistributedCaching(
        this IServiceCollection services,
        CachingOptions options)
    {
        if (!string.IsNullOrEmpty(options.RedisConnectionString))
        {
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = options.RedisConnectionString;
                opt.InstanceName = options.RedisInstanceName;
            });
        }
        else
        {
            // Fallback на in-memory distributed cache для разработки
            services.AddDistributedMemoryCache();
        }

        services.AddSingleton<ICacheService, DistributedCacheService>();
        return services;
    }

    /// <summary>
    /// Добавляет Redis кеширование напрямую через StackExchange.Redis.
    /// Для полной функциональности (pattern delete, pub/sub).
    /// </summary>
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection services,
        CachingOptions options)
    {
        if (string.IsNullOrEmpty(options.RedisConnectionString))
        {
            throw new InvalidOperationException(
                "RedisConnectionString is required when using CacheType.Redis. " +
                "Set it in CachingOptions or appsettings.json.");
        }

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = options.RedisConnectionString;
            opt.InstanceName = options.RedisInstanceName;
        });

        services.AddSingleton<ICacheService, DistributedCacheService>();
        return services;
    }
}
