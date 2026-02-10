using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Queries.GetVolunteerById;

/// <summary>
/// Обработчик запроса на получение волонтёра по идентификатору.
/// </summary>
public class GetVolunteerByIdHandler(IVolunteerService volunteerService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение волонтёра.
    /// </summary>
    public async Task<VolunteerDto> Handle(GetVolunteerByIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.VolunteerById(query.VolunteerId);

        var cached = await cache.GetAsync<VolunteerDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var volunteer = await volunteerService.GetAsync(query.VolunteerId, ct);

        var result = volunteer.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Volunteers);

        return result;
    }
}
