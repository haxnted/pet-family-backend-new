using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.RemoveVolunteer;

/// <summary>
/// Обработчик команды удаления волонтёра из приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class RemoveVolunteerFromShelterHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду удаления волонтёра из приюта.
	/// </summary>
	/// <param name="command">Команда удаления волонтёра.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(RemoveVolunteerFromShelterCommand command, CancellationToken ct)
	{
		await shelterService.RemoveVolunteerAsync(
			command.ShelterId,
			command.VolunteerId,
			ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}