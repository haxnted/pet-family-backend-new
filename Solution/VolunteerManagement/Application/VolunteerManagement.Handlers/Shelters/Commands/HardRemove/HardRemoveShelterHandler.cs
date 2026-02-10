using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.HardRemove;

/// <summary>
/// Обработчик команды полного удаления приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class HardRemoveShelterHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает команду полного удаления приюта.
    /// </summary>
    /// <param name="command">Команда удаления приюта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(HardRemoveShelterCommand command, CancellationToken ct)
    {
        await shelterService.HardRemoveAsync(command.ShelterId, ct);
    }
}
