namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на обновление волонтера.
/// </summary>
/// <param name="Description">Описание волонтёра.</param>
/// <param name="AgeExperience">Опыт работы волонтёром в годах.</param>
/// <param name="PhoneNumber">Контактный номер телефона.</param>
public sealed record UpdateVolunteerRequest(
    string Description,
    int? AgeExperience,
    string? PhoneNumber);