using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Services.Shelters.Dtos;

namespace VolunteerManagement.Handlers.Shelters.Queries.GetShelterById;

/// <summary>
/// Обработчик запроса на получение приюта по идентификатору.
/// </summary>
public class GetShelterByIdHandler(IShelterService shelterService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение приюта.
    /// </summary>
    public async Task<ShelterDto> Handle(GetShelterByIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.ShelterById(query.ShelterId);

        var cached = await cache.GetAsync<ShelterDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var shelter = await shelterService.GetAsync(query.ShelterId, ct);

        var result = shelter.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Shelters);

        return result;
    }
}
