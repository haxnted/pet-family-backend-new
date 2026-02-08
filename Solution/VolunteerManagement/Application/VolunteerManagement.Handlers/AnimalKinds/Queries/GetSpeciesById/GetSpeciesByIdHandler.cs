using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Services.AnimalKinds.Dtos;

namespace VolunteerManagement.Handlers.AnimalKinds.Queries.GetSpeciesById;

/// <summary>
/// Обработчик запроса на получение вида животного по идентификатору.
/// </summary>
public class GetSpeciesByIdHandler(ISpeciesService speciesService)
{
    /// <summary>
    /// Обработать запрос на получение вида по идентификатору.
    /// </summary>
    public async Task<SpeciesDto> Handle(
        GetSpeciesByIdQuery query,
        CancellationToken ct)
    {
        var species = await speciesService.GetByIdAsync(query.SpeciesId, ct);

        var mappedSpecies = species.ToDto();

        return mappedSpecies;
    }
}
