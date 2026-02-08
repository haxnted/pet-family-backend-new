namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// Dto Волонтер.
/// </summary>
/// <param name="Id">Идентификатор волонтера.</param>
/// <param name="FullName">Полное имя.</param>
/// <param name="GeneralDescription">Общее описание.</param>
/// <param name="AgeExperience">Опыт работы (в годах).</param>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="Pets">Коллекция животных.</param>
public record VolunteerDto(
    Guid Id,
    FullNameDto FullName,
    string GeneralDescription,
    int? AgeExperience,
    string? PhoneNumber,
    IEnumerable<PetDto> Pets);
