namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на добавление вида животного.
/// </summary>
/// <param name="AnimalKind">Вид животного.</param>
public sealed record AddSpeciesRequest(string AnimalKind);
