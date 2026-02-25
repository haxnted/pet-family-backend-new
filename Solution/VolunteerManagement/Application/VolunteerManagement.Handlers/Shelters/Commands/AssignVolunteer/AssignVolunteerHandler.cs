using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.AssignVolunteer;

/// <summary>
/// Обработчик команды назначения волонтёра в приют.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class AssignVolunteerHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду назначения волонтёра в приют.
	/// </summary>
	/// <param name="command">Команда назначения.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(AssignVolunteerCommand command, CancellationToken ct)
	{
		await shelterService.AssignVolunteerAsync(
			command.ShelterId,
			command.VolunteerId,
			command.Role,
			ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}