using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Update;

/// <summary>
/// Обработчик команды на обновление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
/// <param name="cache">Сервис кэширования.</param>
public class UpdatePetHandler(IPetService petService, ICacheService cache)
{
	/// <summary>
	/// Обработать команду на обновление животного.
	/// </summary>
	/// <param name="command">Команда на обновление животного.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(UpdatePetCommand command, CancellationToken ct)
	{
		await petService.UpdatePet(
			command.VolunteerId,
			command.PetId,
			command.Description,
			command.HealthInformation,
			command.Weight,
			command.Height,
			command.IsCastrated,
			command.IsVaccinated,
			command.HelpStatus,
			command.Requisites,
			ct);

		await cache.RemoveAsync(CacheKeys.PetById(command.VolunteerId, command.PetId), ct);
		await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(command.VolunteerId), ct);
	}
}
