namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteerById;

/// <summary>
/// Запрос на получение волонтёра по идентификатору.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра.</param>
public sealed record GetVolunteerByIdQuery(Guid VolunteerId);
