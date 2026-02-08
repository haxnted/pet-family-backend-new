namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на перемещение питомца на новую позицию.
/// </summary>
/// <param name="NewPosition">Новая позиция питомца.</param>
public sealed record MovePetRequest(int NewPosition);
