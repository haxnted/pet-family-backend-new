using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteBreed;

/// <summary>
/// Обработчик команды удаления породы.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class DeleteBreedHandler(ISpeciesService speciesService)
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
    }
}
