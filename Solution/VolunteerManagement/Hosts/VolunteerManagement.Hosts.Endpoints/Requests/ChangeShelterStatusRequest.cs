namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на изменение статуса приюта.
/// </summary>
/// <param name="NewStatus">Новый статус (0 = Active, 1 = TemporaryClosed, 2 = Inactive).</param>
public sealed record ChangeShelterStatusRequest(int NewStatus);
