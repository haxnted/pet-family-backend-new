namespace Conversation.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на отправку сообщения в чат.
/// </summary>
/// <param name="Text">Текст сообщения.</param>
/// <param name="ParentMessageId">Идентификатор родительского сообщения (для ответа в ветке).</param>
public record SendMessageRequest(
    string Text,
    Guid? ParentMessageId);
