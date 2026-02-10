using Ardalis.Specification;
using VolunteerManagement.Domain.Aggregates.Shelters;

namespace VolunteerManagement.Services.Shelters.Specifications;

/// <summary>
/// Спецификация получения приютов с пагинацией.
/// </summary>
public sealed class GetWithPaginationSpecification : Specification<Shelter>
{
    /// <summary>
    /// Создаёт спецификацию для получения приютов с пагинацией.
    /// </summary>
    /// <param name="page">Текущая страница.</param>
    /// <param name="count">Количество элементов на странице.</param>
    public GetWithPaginationSpecification(int page, int count)
    {
        Query.Include(s => s.VolunteerAssignments)
            .Skip((page - 1) * count)
            .Take(count);
    }
}
