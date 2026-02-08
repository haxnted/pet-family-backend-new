using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на обновление животного.
/// </summary>
/// <param name="PetId">Идентификатор животного.</param>
/// <param name="GeneralDescription">Общее описание животного.</param>
/// <param name="HealthInformation">Информация о здоровье животного.</param>
/// <param name="Address">Адрес местонахождения животного.</param>
/// <param name="Weight">Вес животного в кг.</param>
/// <param name="Height">Рост животного в см.</param>
/// <param name="PhoneNumber">Контактный номер телефона.</param>
/// <param name="IsCastrated">Признак кастрации.</param>
/// <param name="IsVaccinated">Признак вакцинации.</param>
/// <param name="HelpStatus">Статус помощи.</param>
/// <param name="Requisites">Коллекция реквизитов для пожертвований.</param>
public sealed record UpdatePetRequest(
    Guid PetId,
    string GeneralDescription,
    string HealthInformation,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    int HelpStatus,
    IEnumerable<RequisiteDto> Requisites);