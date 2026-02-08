using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Restore;

/// <summary>
/// Обработчик команды восстановления питомца.
/// </summary>
/// <param name="petService">Сервис для работы с питомцами.</param>
public class RestorePetHandler(IPetService petService)
{
    /// <summary>
    /// Обрабатывает команду восстановления питомца.
    /// </summary>
    /// <param name="command">Команда восстановления питомца.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(RestorePetCommand command, CancellationToken ct)
    {
        await petService.RestorePetAsync(
            command.VolunteerId,
            command.PetId,
            ct);
    }
}
