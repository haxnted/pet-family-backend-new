namespace Conversation.Handlers.Queries.GetChatById;

/// <summary>
/// Запрос на получение чата по идентификатору (с сообщениями).
/// </summary>
/// <param name="ChatId">Идентификатор чата.</param>
public sealed record GetChatByIdQuery(Guid ChatId);
