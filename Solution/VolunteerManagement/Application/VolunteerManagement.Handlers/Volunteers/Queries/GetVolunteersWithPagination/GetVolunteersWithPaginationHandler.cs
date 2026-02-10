using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteersWithPagination;

/// <summary>
/// Обработчик запроса на получение волонтёров с пагинацией.
/// </summary>
public class GetVolunteersWithPaginationHandler(IVolunteerService volunteerService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение волонтёров с пагинацией.
    /// </summary>
    public async Task<IEnumerable<VolunteerDto>> Handle(GetVolunteersWithPaginationQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.VolunteersPagination(query.Page, query.Count);

        var cached = await cache.GetAsync<List<VolunteerDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var volunteers = await volunteerService.GetWithPaginationAsync(query.Page, query.Count, ct);

        var result = volunteers.ToDto().ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Volunteers);

        return result;
    }
}
