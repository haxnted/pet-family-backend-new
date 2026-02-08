using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Services.AnimalKinds.Dtos;
using DomainSpecies = VolunteerManagement.Domain.Aggregates.AnimalKinds.Species;

namespace VolunteerManagement.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга Species в DTO.
/// </summary>
public static class SpeciesMappingExtensions
{
    /// <summary>
    /// Преобразовать сущность Species в DTO.
    /// </summary>
    /// <param name="species">Сущность вида животного.</param>
    /// <returns>DTO вида животного.</returns>
    public static SpeciesDto ToDto(this DomainSpecies species)
    {
        var breeds = species.Breeds
            .Where(b => b.DeletedAt == null)
            .Select(b => b.ToDto());

        return new SpeciesDto(
            species.Id.Value,
            species.AnimalKind.Value,
            breeds);
    }

    /// <summary>
    /// Преобразовать сущность Breed в DTO.
    /// </summary>
    /// <param name="breed">Сущность породы.</param>
    /// <returns>DTO породы.</returns>
    public static BreedDto ToDto(this Breed breed)
    {
        return new BreedDto(
            breed.Id.Value,
            breed.Name.Value,
            breed.SpeciesId.Value);
    }

    /// <summary>
    /// Преобразовать коллекцию Species в коллекцию DTO.
    /// </summary>
    /// <param name="species">Коллекция сущностей видов.</param>
    /// <returns>Коллекция DTO видов.</returns>
    public static IEnumerable<SpeciesDto> ToDto(this IEnumerable<DomainSpecies> species)
    {
        return species.Select(s => s.ToDto());
    }
}
