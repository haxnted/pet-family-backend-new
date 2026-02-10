using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using PetFamily.SharedKernel.Application.Exceptions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Services.Volunteers;

/// <inheritdoc/>
/// <param name="repository">Репозиторий над волонтёрами.</param>
/// <param name="cache">Сервис кеширования.</param>
internal sealed class VolunteerService(IRepository<Volunteer> repository, ICacheService cache) : IVolunteerService
{
    /// <inheritdoc/>
    public async Task AddAsync(
        string name,
        string surname,
        string? patronymic,
        Guid userId,
        CancellationToken ct)
    {
        var volunteerId = VolunteerId.Of(Guid.NewGuid());
        var fullName = FullName.Of(name, surname, patronymic);

        var volunteer = Volunteer.Create(volunteerId, fullName);

        volunteer.SetUserId(userId);

        await repository.AddAsync(volunteer, ct);
    }

    /// <inheritdoc/>
    public async Task HardRemoveAsync(Guid volunteerId, CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        await repository.RemoveAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task SoftRemoveAsync(Guid volunteerId, CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        volunteer.Delete();

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task ActivateAccountVolunteerRequest(Guid volunteerId, CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        volunteer.Restore();

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);
    }

    /// <inheritdoc/>
    public async Task<Volunteer> GetAsync(Guid volunteerId, CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdWithPetsSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        return volunteer;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Volunteer>> GetWithPaginationAsync(
        int page,
        int count,
        CancellationToken ct) =>
        repository.GetAll(new GetWithPaginationSpecification(page, count), ct);

    /// <inheritdoc/>
    public async Task HardRemoveAllPetsAsync(Guid volunteerId, CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdWithPetsSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        volunteer.HardRemoveAllPets();

        await repository.UpdateAsync(volunteer, ct);

        await cache.RemoveAsync(CacheKeys.VolunteerById(volunteerId), ct);
        await cache.RemoveAsync(CacheKeys.PetsByVolunteerId(volunteerId), ct);
    }
}