namespace Conversation.Services.Dtos;

/// <summary>
/// DTO сообщения.
/// </summary>
/// <param name="Id">Идентификатор сообщения.</param>
/// <param name="Text">Текст сообщения.</param>
/// <param name="UserId">Идентификатор пользователя.</param>
/// <param name="ParentMessageId">Идентификатор родительского сообщения.</param>
/// <param name="CreatedAt">Дата создания.</param>
public record MessageDto(
    Guid Id,
    string Text,
    Guid UserId,
    Guid? ParentMessageId,
    DateTime CreatedAt);