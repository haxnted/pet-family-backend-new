using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.ActivateAccount;

/// <summary>
/// Обработчик команды активации аккаунта волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class ActivateAccountVolunteerHandler(IVolunteerService volunteerService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду активации аккаунта волонтёра.
	/// </summary>
	/// <param name="command">Команда активации аккаунта.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(ActivateAccountVolunteerCommand command, CancellationToken ct)
	{
		await volunteerService.ActivateAccountVolunteerRequest(command.VolunteerId, ct);

		await cache.RemoveAsync(CacheKeys.VolunteerById(command.VolunteerId), ct);
	}
}
