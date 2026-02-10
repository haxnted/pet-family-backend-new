using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using VolunteerManagement.Services.Volunteers.Pets.Specifications;

namespace VolunteerManagement.Services.Volunteers.Pets;

/// <inheritdoc/>
internal sealed class PetSearchService(IRepository<Pet> repository) : IPetSearchService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Pet>> SearchAsync(
        string? nickName,
        Guid? speciesId,
        Guid? breedId,
        int? helpStatus,
        Guid? shelterId,
        DateTime? birthDateFrom,
        DateTime? birthDateTo,
        bool? isCastrated,
        bool? isVaccinated,
        double? weightFrom,
        double? weightTo,
        double? heightFrom,
        double? heightTo,
        string? sortBy,
        string? sortDirection,
        int page,
        int count,
        CancellationToken ct)
    {
        var specification = new GetPetsWithFilterSpecification(
            nickName,
            speciesId,
            breedId,
            helpStatus,
            shelterId,
            birthDateFrom,
            birthDateTo,
            isCastrated,
            isVaccinated,
            weightFrom,
            weightTo,
            heightFrom,
            heightTo,
            sortBy,
            sortDirection,
            page,
            count);

        return await repository.GetAll(specification, ct);
    }
}
