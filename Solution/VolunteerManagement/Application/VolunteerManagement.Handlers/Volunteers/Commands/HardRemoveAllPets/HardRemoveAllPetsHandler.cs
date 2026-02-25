using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.HardRemoveAllPets;

/// <summary>
/// Обработчик команды жёсткого удаления всех питомцев волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class HardRemoveAllPetsHandler(IVolunteerService volunteerService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду жёсткого удаления всех питомцев.
	/// </summary>
	/// <param name="command">Команда удаления питомцев.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(HardRemoveAllPetsCommand command, CancellationToken ct)
	{
		await volunteerService.HardRemoveAllPetsAsync(command.VolunteerId, ct);

		await cache.RemoveAsync(CacheKeys.VolunteerById(command.VolunteerId), ct);
		await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(command.VolunteerId), ct);
	}
}
