using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.ChangeStatus;

/// <summary>
/// Обработчик команды изменения статуса приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class ChangeShelterStatusHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает команду изменения статуса приюта.
    /// </summary>
    /// <param name="command">Команда изменения статуса.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(ChangeShelterStatusCommand command, CancellationToken ct)
    {
        await shelterService.ChangeStatusAsync(
            command.ShelterId,
            command.NewStatus,
            ct);
    }
}
