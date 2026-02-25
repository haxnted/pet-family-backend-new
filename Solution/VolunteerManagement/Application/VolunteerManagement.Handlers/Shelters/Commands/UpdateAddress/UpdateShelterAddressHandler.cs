using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.UpdateAddress;

/// <summary>
/// Обработчик команды обновления адреса приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class UpdateShelterAddressHandler(IShelterService shelterService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду обновления адреса приюта.
	/// </summary>
	/// <param name="command">Команда обновления адреса.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(UpdateShelterAddressCommand command, CancellationToken ct)
	{
		await shelterService.UpdateAddressAsync(
			command.ShelterId,
			command.Street,
			command.City,
			command.State,
			command.ZipCode,
			ct);

		await cache.RemoveAsync(CacheKeys.ShelterById(command.ShelterId), ct);
	}
}