using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Delete;

/// <summary>
/// Обработчик команды на удаление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
/// <param name="cache">Сервис кэширования.</param>
public class DeletePetHandler(IPetService petService, ICacheService cache)
{
	/// <summary>
	/// Обработать команду на удаление животного.
	/// </summary>
	/// <param name="command">Команда на удаление животного.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(DeletePetCommand command, CancellationToken ct)
	{
		await petService.DeletePet(
			command.VolunteerId,
			command.PetId,
			ct);

		await cache.RemoveAsync(CacheKeys.PetById(command.VolunteerId, command.PetId), ct);
		await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(command.VolunteerId), ct);
		await cache.RemoveAsync(CacheKeys.VolunteerById(command.VolunteerId), ct);
	}
}
