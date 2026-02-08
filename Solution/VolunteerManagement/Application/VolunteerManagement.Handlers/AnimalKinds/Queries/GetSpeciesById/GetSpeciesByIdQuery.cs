namespace VolunteerManagement.Handlers.AnimalKinds.Queries.GetSpeciesById;

/// <summary>
/// Запрос на получение вида животного по идентификатору.
/// </summary>
/// <param name="SpeciesId">Идентификатор вида.</param>
public sealed record GetSpeciesByIdQuery(Guid SpeciesId);
