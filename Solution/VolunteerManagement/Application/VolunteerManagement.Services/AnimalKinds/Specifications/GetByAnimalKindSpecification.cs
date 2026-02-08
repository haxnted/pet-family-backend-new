using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

namespace VolunteerManagement.Services.AnimalKinds.Specifications;

/// <summary>
/// Спецификация для поиска вида животного по названию.
/// </summary>
public sealed class GetByAnimalKindSpecification : Specification<Species>
{
    /// <summary>
    /// Создаёт спецификацию для поиска вида по названию.
    /// </summary>
    /// <param name="animalKind">Вид животного.</param>
    public GetByAnimalKindSpecification(AnimalKind animalKind)
    {
        Query.Where(s => s.AnimalKind == animalKind);
    }
}
