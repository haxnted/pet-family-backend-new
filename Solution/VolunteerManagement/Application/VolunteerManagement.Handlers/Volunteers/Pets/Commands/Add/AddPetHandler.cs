using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Add;

/// <summary>
/// Обработчик команды на добавление животного.
/// </summary>
/// <param name="petService">Сервис для работы с животными.</param>
public class AddPetHandler(IPetService petService)
{
    /// <summary>
    /// Обработать команду на добавление животного.
    /// </summary>
    /// <param name="command">Команда на добавление животного.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Идентификатор созданного животного.</returns>
    public async Task<Guid> Handle(AddPetCommand command, CancellationToken ct)
    {
        return await petService.AddPet(
            command.VolunteerId,
            command.NickName,
            command.GeneralDescription,
            command.HealthInformation,
            command.BreedId,
            command.SpeciesId,
            command.Weight,
            command.Height,
            command.BirthDate,
            command.IsCastrated,
            command.IsVaccinated,
            command.HelpStatus,
            command.Requisites,
            ct);
    }
}
