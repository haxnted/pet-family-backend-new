namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// Dto Имя Фамилия Отчество.
/// </summary>
/// <param name="Name">Имя.</param>
/// <param name="Surname">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
public record FullNameDto(string Name, string Surname, string? Patronymic);
