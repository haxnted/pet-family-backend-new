namespace Conversation.Services.Caching;

/// <summary>
/// Ключи кеширования для чатов.
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// Ключ кеша чата по идентификатору.
    /// </summary>
    public static string ChatById(Guid chatId) => $"chat:{chatId}";

    /// <summary>
    /// Ключ кеша чатов по идентификатору связанной сущности.
    /// </summary>
    public static string ChatsByLinkedId(Guid linkedId) => $"chats:linked:{linkedId}";
}
