using Conversation.Handlers.MappingExtensions;
using Conversation.Services;
using Conversation.Services.Caching;
using Conversation.Services.Dtos;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Conversation.Handlers.Queries.GetChatsByLinkedId;

/// <summary>
/// Обработчик запроса на получение чатов по идентификатору связанной сущности.
/// </summary>
public class GetChatsByLinkedIdHandler(IChatService chatService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение чатов.
    /// </summary>
    public async Task<List<ChatDto>> Handle(GetChatsByLinkedIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.ChatsByLinkedId(query.LinkedId);

        var cached = await cache.GetAsync<List<ChatDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var chats = await chatService.GetByLinkedIdAsync(query.LinkedId, ct);

        var result = chats.Select(c => c.ToDto()).ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Default);

        return result;
    }
}