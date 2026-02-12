namespace Conversation.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на создание чата.
/// </summary>
/// <param name="Title">Заголовок чата.</param>
/// <param name="Description">Описание чата.</param>
/// <param name="LinkedId">Идентификатор связанной сущности.</param>
public record CreateChatRequest(
    string Title,
    string? Description,
    Guid LinkedId);
