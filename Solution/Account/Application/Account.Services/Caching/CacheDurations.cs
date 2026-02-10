namespace Account.Services.Caching;

/// <summary>
/// Константы времени жизни кеша для данных Account.
/// </summary>
public static class CacheDurations
{
    /// <summary>
    /// Профиль аккаунта — 5 минут.
    /// </summary>
    public static readonly TimeSpan Account = TimeSpan.FromMinutes(5);
}
