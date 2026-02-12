using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.AnimalKinds;
using PetFamily.SharedKernel.Application.Exceptions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Services.Volunteers.Pets;

/// <inheritdoc/>
/// <param name="cache">Сервис кеширования.</param>
internal sealed class PetService(
    IRepository<Volunteer> repository,
    ISpeciesService speciesService,
    ICacheService cache) : IPetService
{
    /// <inheritdoc/>
    public async Task<Guid> AddPet(Guid volunteerId,
        string nickName,
        string description,
        string healthInformation,
        Guid breedId,
        Guid speciesId,
        double weight,
        double height,
        DateTime birthDate,
        bool isCastrated,
        bool isVaccinated,
        int helpStatus,
        IEnumerable<RequisiteDto> requisites,
        CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var isBreedValid = await speciesService.ValidateBreedExistsAsync(
            speciesId, breedId, ct);

        if (!isBreedValid)
        {
            throw new UseCaseException(
                $"Порода с id '{breedId}' не найдена в виде с id '{speciesId}'.");
        }

        var petId = PetId.Of(Guid.NewGuid());
        var petNickName = NickName.Of(nickName);
        var petDescription = Description.Of(description);
        var petHealthInfo = Description.Of(healthInformation);
        var petAttributes = PetPhysicalAttributes.Of(weight, height);
        var petStatus = (HelpStatusPet)helpStatus;
        var petRequisites = requisites.Select(r => Requisite.Of(r.Name, r.Description)).ToList();

        var pet = Pet.Create(
            petId,
            id,
            petNickName,
            petDescription,
            petHealthInfo,
            petAttributes,
            speciesId,
            breedId,
            birthDate,
            isCastrated,
            isVaccinated,
            petStatus,
            petRequisites);

        volunteer.AddPet(pet);
        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);

        return petId.Value;
    }

    /// <inheritdoc/>
    public async Task UpdatePet(
        Guid volunteerId,
        Guid petId,
        string description,
        string healthInformation,
        double weight,
        double height,
        bool isCastrated,
        bool isVaccinated,
        int helpStatus,
        IEnumerable<RequisiteDto> requisites,
        CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var pet = volunteer.GetPetById(PetId.Of(petId));

        var petDescription = Description.Of(description);
        var petHealthInfo = Description.Of(healthInformation);
        var petAttributes = PetPhysicalAttributes.Of(weight, height);
        var petStatus = (HelpStatusPet)helpStatus;
        var petRequisites = requisites.Select(r => Requisite.Of(r.Name, r.Description)).ToList();

        pet.Update(
            petDescription,
            petHealthInfo,
            petAttributes,
            isCastrated,
            isVaccinated,
            petStatus,
            petRequisites);

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetById(volunteerId, petId), ct);
        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task DeletePet(Guid volunteerId, Guid petId, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var pet = volunteer.GetPetById(PetId.Of(petId));
        volunteer.HardRemovePet(pet);

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetById(volunteerId, petId), ct);
        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task<Pet> GetPetById(Guid volunteerId, Guid petId, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        return volunteer.GetPetById(PetId.Of(petId));
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Pet>> GetPetsByVolunteerId(Guid volunteerId, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        return volunteer.Pets;
    }

    /// <inheritdoc/>
    public async Task SoftDeletePetAsync(Guid volunteerId, Guid petId, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var pet = volunteer.GetPetById(PetId.Of(petId));
        volunteer.SoftRemovePet(pet);

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetById(volunteerId, petId), ct);
        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task RestorePetAsync(Guid volunteerId, Guid petId, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var pet = volunteer.GetPetById(PetId.Of(petId));
        volunteer.RestorePet(pet);

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetById(volunteerId, petId), ct);
        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task MovePetAsync(Guid volunteerId, Guid petId, int newPosition, CancellationToken ct)
    {
        var id = VolunteerId.Of(volunteerId);
        var volunteer = await repository.FirstOrDefaultAsync(new GetByIdWithPetsSpecification(id), ct);
        if (volunteer == null)
            throw new EntityNotFoundException<Volunteer>(volunteerId);

        var position = Position.Of(newPosition);
        volunteer.MovePet(PetId.Of(petId), position);

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
    }
}