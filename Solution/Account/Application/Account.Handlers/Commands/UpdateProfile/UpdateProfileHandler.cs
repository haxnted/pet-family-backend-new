using Account.Services;
using Account.Services.Caching;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Account.Handlers.Commands.UpdateProfile;

/// <summary>
/// Обработчик команды обновления профильных данных аккаунта.
/// </summary>
/// <param name="accountService">Сервис для работы с аккаунтами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class UpdateProfileHandler(IAccountService accountService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду обновления профиля.
	/// </summary>
	/// <param name="command">Команда обновления профиля.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(UpdateProfileCommand command, CancellationToken ct)
	{
		await accountService.UpdateProfileAsync(
			command.UserId,
			command.PhoneNumber,
			command.AgeExperience,
			command.Description,
			ct);

		await cache.RemoveAsync(CacheKeys.AccountByUserId(command.UserId), ct);
	}
}