using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на добавление животного.
/// </summary>
/// <param name="NickName">Кличка животного.</param>
/// <param name="GeneralDescription">Общее описание животного.</param>
/// <param name="HealthInformation">Информация о здоровье животного.</param>
/// <param name="BreedId">Идентификатор породы.</param>
/// <param name="SpeciesId">Идентификатор вида животного.</param>
/// <param name="Address">Адрес местонахождения животного.</param>
/// <param name="Weight">Вес животного в кг.</param>
/// <param name="Height">Рост животного в см.</param>
/// <param name="PhoneNumber">Контактный номер телефона.</param>
/// <param name="BirthDate">Дата рождения животного.</param>
/// <param name="IsCastrated">Признак кастрации.</param>
/// <param name="IsVaccinated">Признак вакцинации.</param>
/// <param name="HelpStatus">Статус помощи.</param>
/// <param name="Requisites">Коллекция реквизитов для пожертвований.</param>
public sealed record AddPetRequest(
    string NickName,
    string GeneralDescription,
    string HealthInformation,
    Guid BreedId,
    Guid SpeciesId,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    DateTime BirthDate,
    bool IsCastrated,
    bool IsVaccinated,
    int HelpStatus,
    IEnumerable<RequisiteDto> Requisites);