using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Services.Shelters.Dtos;

namespace VolunteerManagement.Handlers.Shelters.Queries.GetAssignment;

/// <summary>
/// Обработчик запроса на получение назначения волонтёра.
/// </summary>
/// <param name="shelterService">Сервис для работы с приютами.</param>
public class GetAssignmentHandler(IShelterService shelterService)
{
    /// <summary>
    /// Обрабатывает запрос на получение назначения волонтёра.
    /// </summary>
    /// <param name="query">Запрос.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task<VolunteerAssignmentDto> Handle(GetAssignmentQuery query, CancellationToken ct)
    {
        var shelter = await shelterService.GetWithAssignmentAsync(
            query.ShelterId,
            query.AssignmentId,
            ct);

        var assignmentId = VolunteerAssignmentId.Of(query.AssignmentId);
        var assignment = shelter.GetAssignmentById(assignmentId);

        return assignment.ToDto();
    }
}
