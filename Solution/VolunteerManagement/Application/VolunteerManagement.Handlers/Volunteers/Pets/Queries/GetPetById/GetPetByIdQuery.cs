namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetById;

/// <summary>
/// Запрос на получение животного по идентификатору.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра.</param>
/// <param name="PetId">Идентификатор животного.</param>
public sealed record GetPetByIdQuery(Guid VolunteerId, Guid PetId);
