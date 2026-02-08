using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;

namespace VolunteerManagement.Services.AnimalKinds.Specifications;

/// <summary>
/// Спецификация для поиска вида животного по идентификатору.
/// </summary>
public sealed class GetByIdSpecification : Specification<Species>
{
    /// <summary>
    /// Создаёт спецификацию для поиска вида по идентификатору.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    public GetByIdSpecification(SpeciesId speciesId)
    {
        Query.Where(s => s.Id == speciesId)
            .Include(s => s.Breeds);
    }
}