using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.SoftRemove;

/// <summary>
/// Обработчик команды мягкого удаления приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class SoftRemoveShelterHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду мягкого удаления приюта.
	/// </summary>
	/// <param name="command">Команда мягкого удаления приюта.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(SoftRemoveShelterCommand command, CancellationToken ct)
	{
		await shelterService.SoftRemoveAsync(command.ShelterId, ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}