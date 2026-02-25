using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Add;

/// <summary>
/// Обработчик команды на добавление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
/// <param name="cache">Сервис кэширования.</param>
public class AddPetHandler(IPetService petService, ICacheService cache)
{
	/// <summary>
	/// Обработать команду на добавление животного.
	/// </summary>
	/// <param name="command">Команда на добавление животного.</param>
	/// <param name="ct">Токен отмены операции.</param>
	/// <returns>Идентификатор созданного животного.</returns>
	public async Task<Guid> Handle(AddPetCommand command, CancellationToken ct)
	{
		var petId = await petService.AddPet(
			command.VolunteerId,
			command.NickName,
			command.GeneralDescription,
			command.HealthInformation,
			command.BreedId,
			command.SpeciesId,
			command.Weight,
			command.Height,
			command.BirthDate,
			command.IsCastrated,
			command.IsVaccinated,
			command.HelpStatus,
			command.Requisites,
			ct);

		await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(command.VolunteerId), ct);
		await cache.RemoveAsync(CacheKeys.VolunteerById(command.VolunteerId), ct);

		return petId;
	}
}
