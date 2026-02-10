namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// Dto Волонтер.
/// </summary>
/// <param name="Id">Идентификатор волонтера.</param>
/// <param name="FullName">Полное имя.</param>
/// <param name="Pets">Коллекция животных.</param>
public record VolunteerDto(
    Guid Id,
    FullNameDto FullName,
    IEnumerable<PetDto> Pets);
