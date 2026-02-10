using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

namespace VolunteerManagement.Services.Shelters.Specifications;

/// <summary>
/// Спецификация для поиска Приюта по идентификатору.
/// </summary>
public sealed class GetByIdSpecification : Specification<Shelter>
{
    /// <summary>
    /// Создаёт спецификацию для поиска приюта по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    public GetByIdSpecification(ShelterId shelterId)
    {
        Query.Where(shelter => shelter.Id == shelterId);
    }
}
