namespace VolunteerManagement.Services.Volunteers.Dtos;

/// <summary>
/// Dto Адрес.
/// </summary>
/// <param name="Street">Улица.</param>
/// <param name="City">Город.</param>
/// <param name="State">Штат.</param>
/// <param name="ZipCode">Код.</param>
public record AddressDto(string Street, string City, string State, string ZipCode);