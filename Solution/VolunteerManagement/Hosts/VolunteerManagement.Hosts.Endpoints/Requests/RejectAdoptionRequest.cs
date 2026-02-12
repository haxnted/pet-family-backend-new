namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на отклонение усыновления питомца.
/// </summary>
/// <param name="Reason">Причина отклонения.</param>
public record RejectAdoptionRequest(string? Reason);
