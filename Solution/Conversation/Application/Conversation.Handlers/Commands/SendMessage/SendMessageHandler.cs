using Conversation.Services;

namespace Conversation.Handlers.Commands.SendMessage;

/// <summary>
/// Обработчик команды отправки сообщения в чат.
/// </summary>
/// <param name="chatService">Сервис для работы с чатами.</param>
public class SendMessageHandler(IChatService chatService)
{
    /// <summary>
    /// Обрабатывает команду отправки сообщения.
    /// </summary>
    /// <param name="command">Команда отправки сообщения.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного сообщения.</returns>
    public async Task<Guid> Handle(SendMessageCommand command, CancellationToken ct)
    {
        return await chatService.AddMessageAsync(
            command.ChatId,
            command.Text,
            command.UserId,
            command.ParentMessageId,
            ct);
    }
}
