namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на назначение волонтёра в приют.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра.</param>
/// <param name="Role">Роль волонтёра (0 = Manager, 1 = Caretaker, 2 = Veterinarian).</param>
public sealed record AssignVolunteerRequest(Guid VolunteerId, int Role);
