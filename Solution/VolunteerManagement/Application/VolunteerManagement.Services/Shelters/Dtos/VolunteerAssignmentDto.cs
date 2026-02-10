namespace VolunteerManagement.Services.Shelters.Dtos;

/// <summary>
/// Dto Назначение волонтёра.
/// </summary>
/// <param name="Id">Идентификатор назначения.</param>
/// <param name="VolunteerId">Идентификатор волонтёра.</param>
/// <param name="Role">Роль.</param>
/// <param name="AssignedAt">Дата назначения.</param>
/// <param name="IsActive">Активно ли назначение.</param>
public record VolunteerAssignmentDto(
    Guid Id,
    Guid VolunteerId,
    string Role,
    DateTime AssignedAt,
    bool IsActive);
