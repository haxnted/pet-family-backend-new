using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга <see cref="Pet"/> в <see cref="PetDto"/> DTO.
/// </summary>
public static class PetMappingExtensions
{
    /// <summary>
    /// Преобразовать сущность  <see cref="Pet"/> в <see cref="PetDto"/> DTO.
    /// </summary>
    /// <param name="pet">Сущность животного.</param>
    /// <returns>DTO животного.</returns>
    public static PetDto ToDto(this Pet pet)
    {
        var mappedRequisites = pet.RequisiteList
            .Select(r => new RequisiteDto(r.Name, r.Description));

        var mappedPetPhotos = pet.Photos
            .Select(p => new PetPhotoDto(p.Value));

        return new PetDto(
            pet.Id.Value,
            pet.VolunteerId.Value,
            pet.NickName.Value,
            pet.Description.Value,
            pet.HealthInformation.Value,
            pet.BreedId,
            pet.SpeciesId,
            pet.PhysicalAttributes.Weight,
            pet.PhysicalAttributes.Height,
            pet.BirthDate,
            pet.IsCastrated,
            pet.IsVaccinated,
            (int)pet.HelpStatus,
            pet.Position.Value,
            pet.DateCreated,
            mappedRequisites,
            mappedPetPhotos,
            pet.IsSoftDeleted);
    }

    /// <summary>
    /// Преобразовать коллекцию  <see cref="Pet"/> в коллекцию <see cref="PetDto"/> DTO.
    /// </summary>
    /// <param name="pets">Коллекция сущностей животных.</param>
    /// <returns>Коллекция DTO животных.</returns>
    public static IEnumerable<PetDto> ToDto(this IEnumerable<Pet> pets)
    {
        return pets.Select(pet => pet.ToDto());
    }
}