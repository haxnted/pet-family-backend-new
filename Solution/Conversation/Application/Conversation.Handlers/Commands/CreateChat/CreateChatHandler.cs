using Conversation.Services;

namespace Conversation.Handlers.Commands.CreateChat;

/// <summary>
/// Обработчик команды создания чата.
/// </summary>
/// <param name="chatService">Сервис для работы с чатами.</param>
public class CreateChatHandler(IChatService chatService)
{
    /// <summary>
    /// Обрабатывает команду создания чата.
    /// </summary>
    /// <param name="command">Команда создания чата.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного чата.</returns>
    public async Task<Guid> Handle(CreateChatCommand command, CancellationToken ct)
    {
        return await chatService.CreateAsync(
            command.Title,
            command.Description,
            command.LinkedId,
            ct);
    }
}
