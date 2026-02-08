using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using PetFamily.SharedKernel.Application.Exceptions;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Services.Volunteers;

/// <inheritdoc/>
/// <param name="repository">Репозиторий над волонтёрами.</param>
internal sealed class VolunteerService(IRepository<Volunteer> repository) : IVolunteerService
{
    /// <inheritdoc/>
    public async Task AddAsync(
        string name,
        string surname,
        string? patronymic,
        Guid userId,
        string generalDescription,
        CancellationToken ct)
    {
        var volunteerId = VolunteerId.Of(Guid.NewGuid());
        var fullName = FullName.Of(name, surname, patronymic);
        var description = Description.Of(generalDescription);

        var volunteer = Volunteer.Create(volunteerId, fullName, description);

        volunteer.SetUserId(userId);

        await repository.AddAsync(volunteer, ct);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(
        Guid volunteerId,
        string generalDescription,
        int? ageExperience,
        string? phoneNumber,
        CancellationToken ct)
    {
        var volunteerIdValue = VolunteerId.Of(volunteerId);

        var specification = new GetByIdSpecification(volunteerIdValue);

        var volunteer = await repository.FirstOrDefaultAsync(specification, ct);

        if (volunteer == null)
        {
            throw new EntityNotFoundException<Volunteer>(volunteerId);
        }

        var description = Description.Of(generalDescription);
        var experience = ageExperience.HasValue ? AgeExperience.Of(ageExperience.Value) : null;
        var phone = phoneNumber != null ? PhoneNumber.Of(phoneNumber) : null;

        volunteer.UpdateMainInfo(description, experience, phone);

        await repository.UpdateAsync(volunteer, ct);
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
    }
}