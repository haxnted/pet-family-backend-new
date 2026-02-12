using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Handlers.Commands.CreateChat;

/// <summary>
/// Команда на создание чата.
/// </summary>
public sealed class CreateChatCommand : Command
{
    /// <summary>
    /// Заголовок чата.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Описание чата.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Идентификатор связанной сущности.
    /// </summary>
    public required Guid LinkedId { get; init; }
}
