namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на добавление приюта.
/// </summary>
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
public sealed record AddShelterRequest(
    string Name,
    string Street,
    string City,
    string State,
    string ZipCode,
    string PhoneNumber,
    string Description,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    int Capacity);
