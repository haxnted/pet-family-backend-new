using Conversation.Domain.Aggregates.Entities;
using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;
using Conversation.Domain.Aggregates.ValueObjects.Properties;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Domain.Aggregates;

/// <summary>
/// Агрегат Чат.
/// </summary>
public sealed class Chat : Entity<ChatId>
{
    private readonly List<Message> _messages = [];

    /// <summary>
    /// EF Конструктор.
    /// </summary>
    private Chat(ChatId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private Chat(
        ChatId id,
        Title title,
        Description? description,
        Guid linkedId) : base(id)
    {
        Title = title;
        Description = description;
        LinkedId = linkedId;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Заголовок чата.
    /// </summary>
    public Title Title { get; private set; } = null!;

    /// <summary>
    /// Описание чата.
    /// </summary>
    public Description? Description { get; private set; }

    /// <summary>
    /// Универсальный идентификатор связанной сущности.
    /// </summary>
    public Guid LinkedId { get; private set; }

    /// <summary>
    /// Коллекция сообщений чата.
    /// </summary>
    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

    /// <summary>
    /// Дата и время создания чата.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Фабричный метод для создания чата <see cref="Chat"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="description">Описание.</param>
    /// <param name="linkedId">Идентификатор связанной сущности.</param>
    /// <returns>Чат <see cref="Chat"/>.</returns>
    public static Chat Create(
        ChatId id,
        Title title,
        Description? description,
        Guid linkedId)
    {
        return new Chat(id, title, description, linkedId);
    }

    /// <summary>
    /// Добавить сообщение в чат.
    /// </summary>
    /// <param name="id">Идентификатор сообщения.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="parentMessageId">Идентификатор родительского сообщения.</param>
    /// <returns>Созданное сообщение <see cref="Message"/>.</returns>
    public Message AddMessage(
        MessageId id,
        MessageText text,
        Guid userId,
        MessageId? parentMessageId = null)
    {
        var message = Message.Create(id, Id, text, userId, parentMessageId);
        _messages.Add(message);
        return message;
    }
}
