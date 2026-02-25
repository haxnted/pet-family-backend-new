using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.ChangeStatus;

/// <summary>
/// Обработчик команды изменения статуса приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class ChangeShelterStatusHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду изменения статуса приюта.
	/// </summary>
	/// <param name="command">Команда изменения статуса.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(ChangeShelterStatusCommand command, CancellationToken ct)
	{
		await shelterService.ChangeStatusAsync(
			command.ShelterId,
			command.NewStatus,
			ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}