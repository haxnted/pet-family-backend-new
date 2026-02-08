using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;

namespace VolunteerManagement.Services.Volunteers.Specifications;

/// <summary>
/// Спецификация для поиска Волонтёра по идентификатору.
/// </summary>
public sealed class GetByIdSpecification : Specification<Volunteer>
{
    /// <summary>
    /// Создаёт спецификацию для поиска волонтёра по идентификатору.
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    public GetByIdSpecification(VolunteerId volunteerId)
    {
        Query.Where(volunteer => volunteer.Id == volunteerId);
    }
}