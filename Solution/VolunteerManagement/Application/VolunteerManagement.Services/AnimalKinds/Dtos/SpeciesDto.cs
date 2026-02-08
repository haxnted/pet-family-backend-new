namespace VolunteerManagement.Services.AnimalKinds.Dtos;

/// <summary>
/// Dto Вид животного.
/// </summary>
/// <param name="Id">Идентификатор вида.</param>
/// <param name="AnimalKind">Вид животного.</param>
/// <param name="Breeds">Коллекция пород.</param>
public sealed record SpeciesDto(Guid Id, string AnimalKind, IEnumerable<BreedDto> Breeds);