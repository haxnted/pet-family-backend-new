using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteSpecies;

/// <summary>
/// Обработчик команды удаления вида животного.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class DeleteSpeciesHandler(ISpeciesService speciesService)
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
    }
}
