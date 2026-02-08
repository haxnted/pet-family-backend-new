using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Delete;

/// <summary>
/// Обработчик команды на удаление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
public class DeletePetHandler(IPetService petService)
{
    /// <summary>
    /// Обработать команду на удаление животного.
    /// </summary>
    /// <param name="command">Команда на удаление животного.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(DeletePetCommand command, CancellationToken ct)
    {
        await petService.DeletePet(
            command.VolunteerId,
            command.PetId,
            ct);
    }
}