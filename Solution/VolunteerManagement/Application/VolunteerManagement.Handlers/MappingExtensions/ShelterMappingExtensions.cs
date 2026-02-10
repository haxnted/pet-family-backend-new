using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.Entities;
using VolunteerManagement.Services.Shelters.Dtos;

namespace VolunteerManagement.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга Shelter в DTO.
/// </summary>
public static class ShelterMappingExtensions
{
    /// <summary>
    /// Преобразовать сущность Shelter в DTO.
    /// </summary>
    /// <param name="shelter">Сущность приюта.</param>
    /// <returns>DTO приюта.</returns>
    public static ShelterDto ToDto(this Shelter shelter)
    {
        var assignments = shelter.VolunteerAssignments.Select(a => a.ToDto());

        return new ShelterDto(
            shelter.Id.Value,
            shelter.Name.Value,
            shelter.Address.Street,
            shelter.Address.City,
            shelter.Address.State,
            shelter.Address.ZipCode,
            shelter.PhoneNumber.Value,
            shelter.Description.Value,
            shelter.WorkingHours.OpenTime,
            shelter.WorkingHours.CloseTime,
            shelter.Capacity,
            shelter.Status.ToString(),
            assignments);
    }

    /// <summary>
    /// Преобразовать сущность VolunteerAssignment в DTO.
    /// </summary>
    /// <param name="assignment">Сущность назначения.</param>
    /// <returns>DTO назначения.</returns>
    public static VolunteerAssignmentDto ToDto(this VolunteerAssignment assignment)
    {
        return new VolunteerAssignmentDto(
            assignment.Id.Value,
            assignment.VolunteerId,
            assignment.Role.ToString(),
            assignment.AssignedAt,
            assignment.IsActive);
    }

    /// <summary>
    /// Преобразовать коллекцию Shelter в коллекцию DTO.
    /// </summary>
    /// <param name="shelters">Коллекция сущностей приютов.</param>
    /// <returns>Коллекция DTO приютов.</returns>
    public static IEnumerable<ShelterDto> ToDto(this IEnumerable<Shelter> shelters)
    {
        return shelters.Select(shelter => shelter.ToDto());
    }
}
