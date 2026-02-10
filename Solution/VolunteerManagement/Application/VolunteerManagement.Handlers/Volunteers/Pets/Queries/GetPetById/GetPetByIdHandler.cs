using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetById;

/// <summary>
/// Обработчик запроса на получение животного по идентификатору.
/// </summary>
public class GetPetByIdHandler(IPetService petService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение животного.
    /// </summary>
    public async Task<PetDto> Handle(GetPetByIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.PetById(query.VolunteerId, query.PetId);

        var cached = await cache.GetAsync<PetDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var pet = await petService.GetPetById(query.VolunteerId, query.PetId, ct);

        var result = pet.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Volunteers);

        return result;
    }
}
