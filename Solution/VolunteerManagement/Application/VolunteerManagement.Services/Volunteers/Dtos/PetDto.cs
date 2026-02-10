namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// Dto Животное.
/// </summary>
/// <param name="Id">Идентификатор животного.</param>
/// <param name="VolunteerId">Идентификатор волонтера.</param>
/// <param name="NickName">Кличка животного.</param>
/// <param name="GeneralDescription">Описание животного.</param>
/// <param name="HealthInformation">Здоровье животного.</param>
/// <param name="BreedId">Идентификатор породы.</param>
/// <param name="SpeciesId">Идентификатор вида.</param>
/// <param name="Weight">Вес.</param>
/// <param name="Height">Рост.</param>
/// <param name="BirthDate">Дата рождения.</param>
/// <param name="IsCastrated">Кастрирован.</param>
/// <param name="IsVaccinated">Вакцинация.</param>
/// <param name="HelpStatus">Статус помощи.</param>
/// <param name="Position">Позиция.</param>
/// <param name="DateCreated">Дата создания.</param>
/// <param name="Requisites">Коллекция реквизитов.</param>
/// <param name="Photos">Коллекция фотографий.</param>
/// <param name="IsDeleted">Удалено.</param>
public record PetDto(
    Guid Id,
    Guid VolunteerId,
    string NickName,
    string GeneralDescription,
    string HealthInformation,
    Guid BreedId,
    Guid SpeciesId,
    double Weight,
    double Height,
    DateTime BirthDate,
    bool IsCastrated,
    bool IsVaccinated,
    int HelpStatus,
    int Position,
    DateTime DateCreated,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<PetPhotoDto> Photos,
    bool IsDeleted);
