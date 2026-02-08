namespace VolunteerManagement.Services.AnimalKinds.Dtos;

/// <summary>
/// Dto Порода.
/// </summary>
/// <param name="Id">Идентификатор породы.</param>
/// <param name="Name">Название породы.</param>
/// <param name="SpeciesId">Идентификатор вида.</param>
public sealed record BreedDto(Guid Id, string Name, Guid SpeciesId);