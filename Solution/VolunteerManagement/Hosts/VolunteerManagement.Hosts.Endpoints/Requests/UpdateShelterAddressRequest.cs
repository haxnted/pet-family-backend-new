namespace VolunteerManagement.Hosts.Endpoints.Requests;

/// <summary>
/// Запрос на обновление адреса приюта.
/// </summary>
/// <param name="Street">Улица.</param>
/// <param name="City">Город.</param>
/// <param name="State">Регион.</param>
/// <param name="ZipCode">Почтовый индекс.</param>
public sealed record UpdateShelterAddressRequest(
    string Street,
    string City,
    string State,
    string ZipCode);
