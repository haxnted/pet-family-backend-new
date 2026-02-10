using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.SoftRemove;

/// <summary>
/// Обработчик команды мягкого удаления приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class SoftRemoveShelterHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает команду мягкого удаления приюта.
    /// </summary>
    /// <param name="command">Команда мягкого удаления приюта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(SoftRemoveShelterCommand command, CancellationToken ct)
    {
        await shelterService.SoftRemoveAsync(command.ShelterId, ct);
    }
}
