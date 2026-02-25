using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.HardRemove;

/// <summary>
/// Обработчик команды полного удаления приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class HardRemoveShelterHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду полного удаления приюта.
	/// </summary>
	/// <param name="command">Команда удаления приюта.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(HardRemoveShelterCommand command, CancellationToken ct)
	{
		await shelterService.HardRemoveAsync(command.ShelterId, ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}