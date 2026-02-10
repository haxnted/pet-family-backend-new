using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.Update;

/// <summary>
/// Обработчик команды обновления данных приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class UpdateShelterHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает команду обновления данных приюта.
    /// </summary>
    /// <param name="command">Команда обновления приюта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdateShelterCommand command, CancellationToken ct)
    {
        await shelterService.UpdateAsync(
            command.ShelterId,
            command.Name,
            command.PhoneNumber,
            command.Description,
            command.OpenTime,
            command.CloseTime,
            command.Capacity,
            ct);
    }
}
