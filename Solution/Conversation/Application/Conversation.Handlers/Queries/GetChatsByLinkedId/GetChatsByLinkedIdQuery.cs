namespace Conversation.Handlers.Queries.GetChatsByLinkedId;

/// <summary>
/// Запрос на получение чатов по идентификатору связанной сущности.
/// </summary>
/// <param name="LinkedId">Идентификатор связанной сущности.</param>
public sealed record GetChatsByLinkedIdQuery(Guid LinkedId);
