using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.AnimalKinds.Dtos;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Queries.GetAllSpecies;

/// <summary>
/// Обработчик запроса на получение всех видов животных.
/// </summary>
public class GetAllSpeciesHandler(ISpeciesService speciesService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение всех видов.
    /// </summary>
    public async Task<IEnumerable<SpeciesDto>> Handle(
        GetAllSpeciesQuery query,
        CancellationToken ct)
    {
        var cacheKey = CacheKeys.SpeciesAll();

        var cached = await cache.GetAsync<List<SpeciesDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var species = await speciesService.GetAllAsync(ct);

        var result = species.Select(s => s.ToDto()).ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Species);

        return result;
    }
}
