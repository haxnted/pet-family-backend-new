using VolunteerManagement.Services.AnimalKinds;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.Add;

/// <summary>
/// Обработчик команды добавления вида животного.
/// </summary>
/// <param name="speciesService">Сервис для работы с видами животных.</param>
public class AddSpeciesHandler(ISpeciesService speciesService)
{
    /// <summary>
    /// Обрабатывает команду добавления вида животного.
    /// </summary>
    /// <param name="command">Команда добавления вида.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного вида.</returns>
    public async Task<Guid> Handle(AddSpeciesCommand command, CancellationToken ct)
    {
        var speciesId = await speciesService.AddAsync(
            command.AnimalKind,
            ct);

        return speciesId;
    }
}
