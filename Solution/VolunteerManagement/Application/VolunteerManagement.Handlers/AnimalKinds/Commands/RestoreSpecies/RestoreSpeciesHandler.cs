using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreSpecies;

/// <summary>
/// Обработчик команды восстановления вида животного.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
/// <param name="cache">Сервис кэширования.</param>
public class RestoreSpeciesHandler(ISpeciesService speciesService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду восстановления вида животного.
	/// </summary>
	/// <param name="command">Команда восстановления вида.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(RestoreSpeciesCommand command, CancellationToken ct)
	{
		await speciesService.RestoreSpeciesAsync(command.SpeciesId, ct);

		await cache.RemoveAsync(CacheKeys.SpeciesAll(), ct);
		await cache.RemoveAsync(CacheKeys.SpeciesById(command.SpeciesId), ct);
	}
}