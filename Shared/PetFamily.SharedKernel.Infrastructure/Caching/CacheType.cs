namespace PetFamily.SharedKernel.Infrastructure.Caching;

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