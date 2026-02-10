using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetsByVolunteerId;

/// <summary>
/// Обработчик запроса на получение всех животных волонтёра.
/// </summary>
public class GetPetsByVolunteerIdHandler(IPetService petService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение животных волонтёра.
    /// </summary>
    public async Task<List<PetDto>> Handle(GetPetsByVolunteerIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.PetsByVolunteerId(query.VolunteerId);

        var cached = await cache.GetAsync<List<PetDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var pets = await petService.GetPetsByVolunteerId(query.VolunteerId, ct);

        var result = pets.ToDto().ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Volunteers);

        return result;
    }
}
