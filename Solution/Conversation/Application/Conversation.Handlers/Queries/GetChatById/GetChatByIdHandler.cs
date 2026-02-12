using Conversation.Handlers.MappingExtensions;
using Conversation.Services;
using Conversation.Services.Caching;
using Conversation.Services.Dtos;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Conversation.Handlers.Queries.GetChatById;

/// <summary>
/// Обработчик запроса на получение чата по идентификатору.
/// </summary>
public class GetChatByIdHandler(IChatService chatService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение чата.
    /// </summary>
    public async Task<ChatDto> Handle(GetChatByIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.ChatById(query.ChatId);

        var cached = await cache.GetAsync<ChatDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var chat = await chatService.GetByIdAsync(query.ChatId, ct);

        var result = chat.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Default);

        return result;
    }
}