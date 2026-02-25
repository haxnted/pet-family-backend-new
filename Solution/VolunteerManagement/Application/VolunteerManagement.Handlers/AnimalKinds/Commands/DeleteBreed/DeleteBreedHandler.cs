using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteBreed;

/// <summary>
/// Обработчик команды удаления породы.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
/// <param name="cache">Сервис кэширования.</param>
public class DeleteBreedHandler(ISpeciesService speciesService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду удаления породы.
	/// </summary>
	/// <param name="command">Команда удаления породы.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(DeleteBreedCommand command, CancellationToken ct)
	{
		await speciesService.DeleteBreedAsync(
			command.SpeciesId,
			command.BreedId,
			ct);

		await cache.RemoveAsync(CacheKeys.SpeciesAll(), ct);
		await cache.RemoveAsync(CacheKeys.SpeciesById(command.SpeciesId), ct);
	}
}