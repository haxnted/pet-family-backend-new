namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на инициацию усыновления питомца.
/// </summary>
/// <param name="VolunteerId">Идентификатор волонтёра-владельца питомца.</param>
public record InitiateAdoptionRequest(Guid VolunteerId);
