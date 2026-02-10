using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.SearchPets;

/// <summary>
/// Обработчик запроса на поиск животных по фильтрам.
/// </summary>
public class SearchPetsHandler(IPetSearchService petSearchService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на поиск животных.
    /// </summary>
    public async Task<IEnumerable<PetDto>> Handle(SearchPetsQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.PetSearch(
            query.NickName, query.SpeciesId, query.BreedId, query.HelpStatus,
            query.City, query.ShelterId, query.BirthDateFrom, query.BirthDateTo,
            query.IsCastrated, query.IsVaccinated, query.WeightFrom, query.WeightTo,
            query.HeightFrom, query.HeightTo, query.SortBy, query.SortDirection,
            query.Page, query.Count);

        var cached = await cache.GetAsync<List<PetDto>>(cacheKey, ct);
        if (cached != null)
            return cached;

        var pets = await petSearchService.SearchAsync(
            query.NickName, query.SpeciesId, query.BreedId, query.HelpStatus,
            query.ShelterId, query.BirthDateFrom, query.BirthDateTo,
            query.IsCastrated, query.IsVaccinated, query.WeightFrom, query.WeightTo,
            query.HeightFrom, query.HeightTo, query.SortBy, query.SortDirection,
            query.Page, query.Count, ct);

        var result = pets.ToDto().ToList();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.PetSearch);

        return result;
    }
}