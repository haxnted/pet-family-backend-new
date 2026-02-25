using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteSpecies;

/// <summary>
/// Обработчик команды удаления вида животного.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
/// <param name="cache">Сервис кэширования.</param>
public class DeleteSpeciesHandler(ISpeciesService speciesService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду удаления вида животного.
	/// </summary>
	/// <param name="command">Команда удаления вида.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(DeleteSpeciesCommand command, CancellationToken ct)
	{
		await speciesService.DeleteSpeciesAsync(
			command.SpeciesId,
			ct);

		await cache.RemoveAsync(CacheKeys.SpeciesAll(), ct);
		await cache.RemoveAsync(CacheKeys.SpeciesById(command.SpeciesId), ct);
	}
}