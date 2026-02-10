using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

namespace VolunteerManagement.Services.Shelters.Specifications;

/// <summary>
/// Спецификация для поиска Приюта с назначениями волонтёров по идентификатору.
/// </summary>
public sealed class GetByIdWithAssignmentsSpecification : Specification<Shelter>
{
    /// <summary>
    /// Создаёт спецификацию для поиска приюта с назначениями по идентификатору.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    public GetByIdWithAssignmentsSpecification(ShelterId shelterId)
    {
        Query.Include(s => s.VolunteerAssignments)
            .Where(shelter => shelter.Id == shelterId);
    }
}
