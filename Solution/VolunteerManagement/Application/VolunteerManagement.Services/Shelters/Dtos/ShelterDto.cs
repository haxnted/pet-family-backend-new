namespace VolunteerManagement.Services.Shelters.Dtos;

/// <summary>
/// Dto Приют.
/// </summary>
/// <param name="Id">Идентификатор приюта.</param>
/// <param name="Name">Название.</param>
/// <param name="Street">Улица.</param>
/// <param name="City">Город.</param>
/// <param name="State">Регион.</param>
/// <param name="ZipCode">Почтовый индекс.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="Description">Описание.</param>
/// <param name="OpenTime">Время открытия.</param>
/// <param name="CloseTime">Время закрытия.</param>
/// <param name="Capacity">Вместимость.</param>
/// <param name="Status">Статус.</param>
/// <param name="Assignments">Назначения волонтёров.</param>
public record ShelterDto(
    Guid Id,
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode,
    string PhoneNumber,
    string Description,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    int Capacity,
    string Status,
    IEnumerable<VolunteerAssignmentDto> Assignments);
