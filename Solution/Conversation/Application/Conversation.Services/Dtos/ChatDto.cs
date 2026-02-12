namespace Conversation.Services.Dtos;

/// <summary>
/// DTO чата.
/// </summary>
/// <param name="Id">Идентификатор чата.</param>
/// <param name="Title">Заголовок.</param>
/// <param name="Description">Описание.</param>
/// <param name="LinkedId">Идентификатор связанной сущности.</param>
/// <param name="CreatedAt">Дата создания.</param>
/// <param name="Messages">Список сообщений.</param>
public record ChatDto(
    Guid Id,
    string Title,
    string? Description,
    Guid LinkedId,
    DateTime CreatedAt,
    List<MessageDto> Messages);