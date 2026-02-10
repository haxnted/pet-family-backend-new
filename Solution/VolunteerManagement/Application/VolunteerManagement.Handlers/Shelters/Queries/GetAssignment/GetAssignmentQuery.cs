namespace VolunteerManagement.Handlers.Shelters.Queries.GetAssignment;

/// <summary>
/// Запрос на получение назначения волонтёра по идентификатору.
/// </summary>
/// <param name="ShelterId">Идентификатор приюта.</param>
/// <param name="AssignmentId">Идентификатор назначения.</param>
public sealed record GetAssignmentQuery(Guid ShelterId, Guid AssignmentId);
