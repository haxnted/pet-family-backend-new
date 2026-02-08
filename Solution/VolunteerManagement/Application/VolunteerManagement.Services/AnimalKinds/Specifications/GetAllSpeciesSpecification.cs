using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;

namespace VolunteerManagement.Services.AnimalKinds.Specifications;

/// <summary>
/// Спецификация для получения всех видов животных.
/// </summary>
public sealed class GetAllSpeciesSpecification : Specification<Species>
{
    /// <summary>
    /// Создаёт спецификацию для получения всех видов.
    /// </summary>
    public GetAllSpeciesSpecification()
    {
        Query.Include(s => s.Breeds)
            .OrderBy(s => s.AnimalKind);
    }
}
