using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Update;

/// <summary>
/// Обработчик команды на обновление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
public class UpdatePetHandler(IPetService petService)
{
    /// <summary>
    /// Обработать команду на обновление животного.
    /// </summary>
    /// <param name="command">Команда на обновление животного.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdatePetCommand command, CancellationToken ct)
    {
        await petService.UpdatePet(
            command.VolunteerId,
            command.PetId,
            command.Description,
            command.HealthInformation,
            command.Weight,
            command.Height,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            command.Requisites,
            ct);
    }
}
