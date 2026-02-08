using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;

namespace VolunteerManagement.Services.Volunteers.Specifications;

/// <summary>
/// Спецификация для поиска Волонтёра c животными по идентификатору.
/// </summary>
public sealed class GetByIdWithPetsSpecification : Specification<Volunteer>
{
    /// <summary>
    /// Создаёт спецификацию для поиска волонтёра с животными по идентификатору.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    public GetByIdWithPetsSpecification(VolunteerId volunteerId)
    {
        Query.Include(p => p.Pets)
            .Where(volunteer => volunteer.Id == volunteerId);
    }
}