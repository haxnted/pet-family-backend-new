using Ardalis.Specification;
using Conversation.Domain.Aggregates;
using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;

namespace Conversation.Services.Specifications;

/// <summary>
/// Спецификация для получения чата по идентификатору с сообщениями.
/// </summary>
public sealed class GetByIdWithMessagesSpecification : Specification<Chat>
{
    /// <summary>
    /// Инициализирует спецификацию.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    public GetByIdWithMessagesSpecification(ChatId chatId)
    {
        Query.Where(c => c.Id == chatId)
            .Include(c => c.Messages);
    }
}