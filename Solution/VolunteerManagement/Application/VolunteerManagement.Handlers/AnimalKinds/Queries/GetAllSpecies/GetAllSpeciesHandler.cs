using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.AnimalKinds.Dtos;

namespace VolunteerManagement.Handlers.AnimalKinds.Queries.GetAllSpecies;

/// <summary>
/// Обработчик запроса на получение всех видов животных.
/// </summary>
public class GetAllSpeciesHandler(ISpeciesService speciesService)
{
    /// <summary>
    /// Обработать запрос на получение всех видов.
    /// </summary>
    public async Task<IEnumerable<SpeciesDto>> Handle(
        GetAllSpeciesQuery query,
        CancellationToken ct)
    {
        var species = await speciesService.GetAllAsync(ct);

        var mappedSpecies = species.Select(s => s.ToDto());

        return mappedSpecies;
    }
}
