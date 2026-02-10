using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Services.Shelters.Dtos;

namespace VolunteerManagement.Handlers.Shelters.Queries.GetSheltersWithPagination;

/// <summary>
/// Обработчик запроса на получение приютов с пагинацией.
/// </summary>
public class GetSheltersWithPaginationHandler(IShelterService shelterService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение приютов с пагинацией.
    /// </summary>
    public async Task<IEnumerable<ShelterDto>> Handle(
        GetSheltersWithPaginationQuery query,
        CancellationToken ct)
    {
        var cacheKey = CacheKeys.SheltersPagination(query.Page, query.Count);

        var cached = await cache.GetAsync<List<ShelterDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var shelters = await shelterService.GetWithPaginationAsync(query.Page, query.Count, ct);

        var result = shelters.ToDto().ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Shelters);

        return result;
    }
}
