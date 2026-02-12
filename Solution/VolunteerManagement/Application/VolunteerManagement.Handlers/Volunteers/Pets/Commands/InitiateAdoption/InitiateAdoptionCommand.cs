namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.InitiateAdoption;

/// <summary>
/// Команда на инициацию усыновления питомца.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра-владельца питомца.</param>
/// <param name="PetId">Идентификатор питомца.</param>
/// <param name="AdopterId">Идентификатор усыновителя (текущий пользователь).</param>
/// <param name="AdopterName">Имя усыновителя (текущий пользователь).</param>
public sealed record InitiateAdoptionCommand(
    Guid VolunteerId,
    Guid PetId,
    Guid AdopterId,
    string AdopterName);
