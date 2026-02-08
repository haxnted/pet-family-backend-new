namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetsByVolunteerId;

/// <summary>
/// Запрос на получение всех животных волонтёра.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра.</param>
public sealed record GetPetsByVolunteerIdQuery(Guid VolunteerId);
