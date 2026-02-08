using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.AddBreed;

/// <summary>
/// Обработчик команды добавления породы.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class AddBreedHandler(ISpeciesService speciesService)
{
    /// <summary>
    /// Обрабатывает команду добавления породы.
    /// </summary>
    /// <param name="command">Команда добавления породы.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Идентификатор созданной породы.</returns>
    public async Task<Guid> Handle(AddBreedCommand command, CancellationToken ct)
    {
        var breedId = await speciesService.AddBreedAsync(
            command.SpeciesId,
            command.BreedName,
            ct);

        return breedId;
    }
}
