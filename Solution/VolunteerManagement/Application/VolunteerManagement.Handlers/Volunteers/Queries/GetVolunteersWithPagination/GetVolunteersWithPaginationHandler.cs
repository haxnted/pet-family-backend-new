using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteersWithPagination;

/// <summary>
/// Обработчик запроса на получение волонтёров с пагинацией.
/// </summary>
public class GetVolunteersWithPaginationHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обработать запрос на получение волонтёров с пагинацией.
    /// </summary>
    public async Task<IEnumerable<VolunteerDto>> Handle(GetVolunteersWithPaginationQuery query, CancellationToken ct)
    {
        var volunteers = await volunteerService.GetWithPaginationAsync(query.Page, query.Count, ct);

        var mappedVolunteers = volunteers.ToDto();

        return mappedVolunteers;
    }
}