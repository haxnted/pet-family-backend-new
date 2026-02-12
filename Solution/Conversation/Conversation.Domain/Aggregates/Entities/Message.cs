using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;
using Conversation.Domain.Aggregates.ValueObjects.Properties;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Domain.Aggregates.Entities;

/// <summary>
/// Сущность Сообщение в чате.
/// </summary>
public sealed class Message : Entity<MessageId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    private Message(MessageId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private Message(
        MessageId id,
        ChatId chatId,
        MessageText text,
        Guid userId,
        MessageId? parentMessageId) : base(id)
    {
        ChatId = chatId;
        Text = text;
        UserId = userId;
        ParentMessageId = parentMessageId;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Идентификатор чата, к которому принадлежит сообщение.
    /// </summary>
    public ChatId ChatId { get; private set; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public MessageText Text { get; private set; } = null!;

    /// <summary>
    /// Идентификатор пользователя, отправившего сообщение.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Идентификатор родительского сообщения (для древовидной структуры).
    /// </summary>
    public MessageId? ParentMessageId { get; private set; }

    /// <summary>
    /// Дата и время создания сообщения.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Фабричный метод для создания сообщения <see cref="Message"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="parentMessageId">Идентификатор родительского сообщения.</param>
    /// <returns>Сообщение <see cref="Message"/>.</returns>
    public static Message Create(
        MessageId id,
        ChatId chatId,
        MessageText text,
        Guid userId,
        MessageId? parentMessageId = null)
    {
        return new Message(id, chatId, text, userId, parentMessageId);
    }
}