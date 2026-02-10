using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;
using PetFamily.SharedKernel.Application.Exceptions;
using VolunteerManagement.Domain.Aggregates.Shelters;
using VolunteerManagement.Domain.Aggregates.Shelters.Entities;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters.Specifications;

namespace VolunteerManagement.Services.Shelters;

/// <inheritdoc/>
/// <param name="repository">Репозиторий над приютами.</param>
/// <param name="cache">Сервис кеширования.</param>
internal sealed class ShelterService(IRepository<Shelter> repository, ICacheService cache) : IShelterService
{
    /// <inheritdoc/>
    public async Task AddAsync(
        string name,
        string street,
        string city,
        string state,
        string zipCode,
        string phoneNumber,
        string description,
        TimeOnly openTime,
        TimeOnly closeTime,
        int capacity,
        CancellationToken ct)
    {
        var shelterId = ShelterId.Of(Guid.NewGuid());
        var shelterName = ShelterName.Of(name);
        var address = Address.Of(street, city, state, zipCode);
        var phone = PhoneNumber.Of(phoneNumber);
        var desc = Description.Of(description);
        var workingHours = WorkingHours.Of(openTime, closeTime);

        var shelter = Shelter.Create(shelterId, shelterName, address, phone, desc, workingHours, capacity);

        await repository.AddAsync(shelter, ct);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(
        Guid shelterId,
        string name,
        string phoneNumber,
        string description,
        TimeOnly openTime,
        TimeOnly closeTime,
        int capacity,
        CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        var shelterName = ShelterName.Of(name);
        var phone = PhoneNumber.Of(phoneNumber);
        var desc = Description.Of(description);
        var workingHours = WorkingHours.Of(openTime, closeTime);

        shelter.UpdateMainInfo(shelterName, desc, phone, workingHours, capacity);

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task HardRemoveAsync(Guid shelterId, CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        await repository.RemoveAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task SoftRemoveAsync(Guid shelterId, CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        shelter.Delete();

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task<Shelter> GetAsync(Guid shelterId, CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdWithAssignmentsSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        return shelter;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Shelter>> GetWithPaginationAsync(
        int page,
        int count,
        CancellationToken ct) =>
        repository.GetAll(new GetWithPaginationSpecification(page, count), ct);

    /// <inheritdoc/>
    public async Task UpdateAddressAsync(
        Guid shelterId,
        string street,
        string city,
        string state,
        string zipCode,
        CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        var address = Address.Of(street, city, state, zipCode);

        shelter.UpdateAddress(address);

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task ChangeStatusAsync(Guid shelterId, int newStatus, CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        var status = (ShelterStatus)newStatus;

        shelter.ChangeStatus(status);

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task AssignVolunteerAsync(
        Guid shelterId,
        Guid volunteerId,
        int role,
        CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdWithAssignmentsSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        var assignmentId = VolunteerAssignmentId.Of(Guid.NewGuid());
        var volunteerRole = (VolunteerRole)role;

        var assignment = VolunteerAssignment.Create(assignmentId, volunteerId, volunteerRole);

        shelter.AssignVolunteer(assignment);

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task RemoveVolunteerAsync(Guid shelterId, Guid volunteerId, CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdWithAssignmentsSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        shelter.RemoveVolunteer(volunteerId);

        await repository.UpdateAsync(shelter, ct);

        await cache.RemoveAsync(CacheKeys.ShelterById(shelterId), ct);
    }

    /// <inheritdoc/>
    public async Task<Shelter> GetWithAssignmentAsync(
        Guid shelterId,
        Guid assignmentId,
        CancellationToken ct)
    {
        var shelterIdValue = ShelterId.Of(shelterId);

        var specification = new GetByIdWithAssignmentsSpecification(shelterIdValue);

        var shelter = await repository.FirstOrDefaultAsync(specification, ct);

        if (shelter == null)
        {
            throw new EntityNotFoundException<Shelter>(shelterId);
        }

        return shelter;
    }
}
