using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Handlers.Commands.SendMessage;

/// <summary>
/// Команда на отправку сообщения в чат.
/// </summary>
public sealed class SendMessageCommand : Command
{
    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    public required Guid ChatId { get; init; }

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор родительского сообщения.
    /// </summary>
    public Guid? ParentMessageId { get; init; }
}