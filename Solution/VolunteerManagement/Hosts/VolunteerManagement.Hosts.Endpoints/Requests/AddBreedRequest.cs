namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на добавление породы.
/// </summary>
/// <param name="BreedName">Название породы.</param>
public sealed record AddBreedRequest(string BreedName);
