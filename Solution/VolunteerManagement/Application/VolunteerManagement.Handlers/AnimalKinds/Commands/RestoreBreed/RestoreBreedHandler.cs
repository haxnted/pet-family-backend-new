using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreBreed;

/// <summary>
/// Обработчик команды восстановления породы.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class RestoreBreedHandler(ISpeciesService speciesService)
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
    }
}
