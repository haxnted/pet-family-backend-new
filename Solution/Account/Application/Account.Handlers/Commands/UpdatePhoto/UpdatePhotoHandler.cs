using Account.Services;
using Account.Services.Caching;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Account.Handlers.Commands.UpdatePhoto;

/// <summary>
/// Обработчик команды обновления фотографии профиля.
/// </summary>
/// <param name="accountService">Сервис для работы с аккаунтами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class UpdatePhotoHandler(IAccountService accountService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду обновления фотографии.
	/// </summary>
	/// <param name="command">Команда обновления фотографии.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(UpdatePhotoCommand command, CancellationToken ct)
	{
		await accountService.UpdatePhotoAsync(
			command.UserId,
			command.PhotoId,
			ct);

		await cache.RemoveAsync(CacheKeys.AccountByUserId(command.UserId), ct);
	}
}