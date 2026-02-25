using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreBreed;

/// <summary>
/// Обработчик команды восстановления породы.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
/// <param name="cache">Сервис кэширования.</param>
public class RestoreBreedHandler(ISpeciesService speciesService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду восстановления породы.
	/// </summary>
	/// <param name="command">Команда восстановления породы.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(RestoreBreedCommand command, CancellationToken ct)
	{
		await speciesService.RestoreBreedAsync(
			command.SpeciesId,
			command.BreedId,
			ct);

		await cache.RemoveAsync(CacheKeys.SpeciesAll(), ct);
		await cache.RemoveAsync(CacheKeys.SpeciesById(command.SpeciesId), ct);
	}
}