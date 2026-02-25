using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Restore;

/// <summary>
/// Обработчик команды восстановления питомца.
/// </summary>
/// <param name="petService">Сервис для работы с питомцами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class RestorePetHandler(IPetService petService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду восстановления питомца.
	/// </summary>
	/// <param name="command">Команда восстановления питомца.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(RestorePetCommand command, CancellationToken ct)
	{
		await petService.RestorePetAsync(
			command.VolunteerId,
			command.PetId,
			ct);

		await cache.RemoveAsync(CacheKeys.PetById(command.VolunteerId, command.PetId), ct);
		await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(command.VolunteerId), ct);
	}
}
