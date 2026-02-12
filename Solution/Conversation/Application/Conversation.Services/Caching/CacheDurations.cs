namespace Conversation.Services.Caching;

/// <summary>
/// Длительности кеширования.
/// </summary>
public static class CacheDurations
{
    /// <summary>
    /// Длительность кеширования по умолчанию — 5 минут.
    /// </summary>
    public static readonly TimeSpan Default = TimeSpan.FromMinutes(5);
}
