namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на обновление основной информации приюта.
/// </summary>
/// <param name="Name">Название.</param>
/// <param name="PhoneNumber">Номер телефона.</param>
/// <param name="Description">Описание.</param>
/// <param name="OpenTime">Время открытия.</param>
/// <param name="CloseTime">Время закрытия.</param>
/// <param name="Capacity">Вместимость.</param>
public sealed record UpdateShelterRequest(
    string Name,
    string PhoneNumber,
    string Description,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    int Capacity);
