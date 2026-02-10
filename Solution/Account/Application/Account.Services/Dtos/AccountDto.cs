namespace Account.Services.Dtos;

/// <summary>
/// DTO Аккаунт (профиль пользователя).
/// </summary>
/// <param name="Id">Идентификатор аккаунта.</param>
/// <param name="UserId">Идентификатор пользователя из Auth.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="AgeExperience">Опыт (в годах).</param>
/// <param name="Description">Описание профиля.</param>
/// <param name="PhotoId">Идентификатор фотографии.</param>
public record AccountDto(
    Guid Id,
    Guid UserId,
    string? PhoneNumber,
    int? AgeExperience,
    string? Description,
    Guid? PhotoId);
