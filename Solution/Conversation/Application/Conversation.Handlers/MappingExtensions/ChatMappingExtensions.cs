using Conversation.Domain.Aggregates;
using Conversation.Domain.Aggregates.Entities;
using Conversation.Services.Dtos;

namespace Conversation.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга Chat и Message в DTO.
/// </summary>
public static class ChatMappingExtensions
{
    /// <summary>
    /// Преобразовать агрегат <see cref="Chat"/> в DTO <see cref="ChatDto"/>.
    /// </summary>
    public static ChatDto ToDto(this Chat chat)
    {
        return new ChatDto(
            chat.Id.Value,
            chat.Title.Value,
            chat.Description?.Value,
            chat.LinkedId,
            chat.CreatedAt,
            chat.Messages.Select(m => m.ToDto()).ToList());
    }

    /// <summary>
    /// Преобразовать сущность <see cref="Message"/> в DTO <see cref="MessageDto"/>.
    /// </summary>
    public static MessageDto ToDto(this Message message)
    {
        return new MessageDto(
            message.Id.Value,
            message.Text.Value,
            message.UserId,
            message.ParentMessageId?.Value,
            message.CreatedAt);
    }
}