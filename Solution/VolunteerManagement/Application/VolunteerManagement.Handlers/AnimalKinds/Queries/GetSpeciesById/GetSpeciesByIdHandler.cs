using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.AnimalKinds.Dtos;
using VolunteerManagement.Services.Caching;

namespace VolunteerManagement.Handlers.AnimalKinds.Queries.GetSpeciesById;

/// <summary>
/// Обработчик запроса на получение вида животного по идентификатору.
/// </summary>
public class GetSpeciesByIdHandler(ISpeciesService speciesService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение вида по идентификатору.
    /// </summary>
    public async Task<SpeciesDto> Handle(
        GetSpeciesByIdQuery query,
        CancellationToken ct)
    {
        var cacheKey = CacheKeys.SpeciesById(query.SpeciesId);

        var cached = await cache.GetAsync<SpeciesDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var species = await speciesService.GetByIdAsync(query.SpeciesId, ct);

        var result = species.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Species);

        return result;
    }
}
