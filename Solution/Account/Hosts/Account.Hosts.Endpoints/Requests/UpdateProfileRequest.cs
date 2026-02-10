namespace Account.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на обновление профильных данных.
/// </summary>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="AgeExperience">Опыт (в годах).</param>
/// <param name="Description">Описание профиля.</param>
public sealed record UpdateProfileRequest(
    string? PhoneNumber,
    int? AgeExperience,
    string? Description);
