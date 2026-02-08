using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга Volunteer в DTO.
/// </summary>
public static class VolunteerMappingExtensions
{
    /// <summary>
    /// Преобразовать сущность Volunteer в DTO.
    /// </summary>
    /// <param name="volunteer">Сущность волонтера.</param>
    /// <returns>DTO волонтера.</returns>
    public static VolunteerDto ToDto(this Volunteer volunteer)
    {
        var fullNameDto = new FullNameDto(
            volunteer.FullName.Name,
            volunteer.FullName.Surname,
            volunteer.FullName.Patronymic);

        var mappedPets = volunteer.Pets.Select(pet => pet.ToDto());

        return new VolunteerDto(
            volunteer.Id.Value,
            fullNameDto,
            volunteer.GeneralDescription.Value,
            volunteer.AgeExperience?.Value,
            volunteer.PhoneNumber?.Value,
            mappedPets);
    }

    /// <summary>
    /// Преобразовать коллекцию Volunteer в коллекцию DTO.
    /// </summary>
    /// <param name="volunteers">Коллекция сущностей волонтеров.</param>
    /// <returns>Коллекция DTO волонтеров.</returns>
    public static IEnumerable<VolunteerDto> ToDto(this IEnumerable<Volunteer> volunteers)
    {
        return volunteers.Select(volunteer => volunteer.ToDto());
    }
}