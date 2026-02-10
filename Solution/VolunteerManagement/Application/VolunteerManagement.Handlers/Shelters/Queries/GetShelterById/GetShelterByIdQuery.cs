namespace VolunteerManagement.Handlers.Shelters.Queries.GetShelterById;

/// <summary>
/// Запрос на получение приюта по идентификатору.
/// </summary>
/// <param name="ShelterId">Идентификатор приюта.</param>
public sealed record GetShelterByIdQuery(Guid ShelterId);
