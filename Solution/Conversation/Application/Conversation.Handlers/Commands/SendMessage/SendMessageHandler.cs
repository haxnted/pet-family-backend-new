using Conversation.Services;
using Conversation.Services.Caching;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Conversation.Handlers.Commands.SendMessage;

/// <summary>
/// Обработчик команды отправки сообщения в чат.
/// </summary>
/// <param name="chatService">Сервис для работы с чатами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class SendMessageHandler(IChatService chatService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду отправки сообщения.
	/// </summary>
	/// <param name="command">Команда отправки сообщения.</param>
	/// <param name="ct">Токен отмены операции.</param>
	/// <returns>Идентификатор созданного сообщения.</returns>
	public async Task<Guid> Handle(SendMessageCommand command, CancellationToken ct)
	{
		var messageId = await chatService.AddMessageAsync(
			command.ChatId,
			command.Text,
			command.UserId,
			command.ParentMessageId,
			ct);

		await cache.RemoveAsync(CacheKeys.ChatById(command.ChatId), ct);

		return messageId;
	}
}