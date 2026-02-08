using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Move;

/// <summary>
/// Обработчик команды перемещения питомца.
/// </summary>
/// <param name="petService">Сервис для работы с питомцами.</param>
public class MovePetHandler(IPetService petService)
{
    /// <summary>
    /// Обрабатывает команду перемещения питомца на новую позицию.
    /// </summary>
    /// <param name="command">Команда перемещения питомца.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(MovePetCommand command, CancellationToken ct)
    {
        await petService.MovePetAsync(
            command.VolunteerId,
            command.PetId,
            command.NewPosition,
            ct);
    }
}
