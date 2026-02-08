using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreSpecies;

/// <summary>
/// Обработчик команды восстановления вида животного.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class RestoreSpeciesHandler(ISpeciesService speciesService)
{
    /// <summary>
    /// Обрабатывает команду восстановления вида животного.
    /// </summary>
    /// <param name="command">Команда восстановления вида.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(RestoreSpeciesCommand command, CancellationToken ct)
    {
        await speciesService.RestoreSpeciesAsync(command.SpeciesId, ct);
    }
}
