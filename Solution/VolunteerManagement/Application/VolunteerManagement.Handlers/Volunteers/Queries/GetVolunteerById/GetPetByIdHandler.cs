using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteerById;

/// <summary>
/// Обработчик запроса на получение волонтёра по идентификатору.
/// </summary>
public class GetVolunteerByIdHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обработать запрос на получение волонтёра.
    /// </summary>
    public async Task<VolunteerDto> Handle(GetVolunteerByIdQuery query, CancellationToken ct)
    {
        var volunteer = await volunteerService.GetAsync(query.VolunteerId, ct);

        var mappedVolunteer = volunteer.ToDto();

        return mappedVolunteer;
    }
}