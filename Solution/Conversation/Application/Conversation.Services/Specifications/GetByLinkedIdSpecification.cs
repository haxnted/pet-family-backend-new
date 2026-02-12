using Ardalis.Specification;
using Conversation.Domain.Aggregates;

namespace Conversation.Services.Specifications;

/// <summary>
/// Спецификация для получения чатов по идентификатору связанной сущности.
/// </summary>
public sealed class GetByLinkedIdSpecification : Specification<Chat>
{
    /// <summary>
    /// Инициализирует спецификацию.
    /// </summary>
    /// <param name="linkedId">Идентификатор связанной сущности.</param>
    public GetByLinkedIdSpecification(Guid linkedId)
    {
        Query.Where(c => c.LinkedId == linkedId);
    }
}