namespace Account.Services.Caching;

/// <summary>
/// Генерация ключей кеша для сущностей Account.
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Ключ кеша для аккаунта по идентификатору пользователя.
    /// </summary>
    public static string AccountByUserId(Guid userId) => $"account:user:{userId}";
}
