using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.SoftDelete;

/// <summary>
/// Обработчик команды мягкого удаления питомца.
/// </summary>
/// <param name="petService">Сервис для работы с питомцами.</param>
public class SoftDeletePetHandler(IPetService petService)
{
    /// <summary>
    /// Обрабатывает команду мягкого удаления питомца.
    /// </summary>
    /// <param name="command">Команда удаления питомца.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(SoftDeletePetCommand command, CancellationToken ct)
    {
        await petService.SoftDeletePetAsync(
            command.VolunteerId,
            command.PetId,
            ct);
    }
}
