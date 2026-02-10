using VolunteerManagement.Services.Shelters;

namespace VolunteerManagement.Handlers.Shelters.Commands.Add;

/// <summary>
/// Обработчик команды добавления приюта.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class AddShelterHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает команду добавления приюта.
    /// </summary>
    /// <param name="command">Команда добавления приюта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(AddShelterCommand command, CancellationToken ct)
    {
        await shelterService.AddAsync(
            command.Name,
            command.Street,
            command.City,
            command.State,
            command.ZipCode,
            command.PhoneNumber,
            command.Description,
            command.OpenTime,
            command.CloseTime,
            command.Capacity,
            ct);
    }
}
