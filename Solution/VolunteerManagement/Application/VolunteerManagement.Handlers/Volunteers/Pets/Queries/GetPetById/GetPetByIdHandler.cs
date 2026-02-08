using VolunteerManagement.Handlers.MappingExtensions;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetPetById;

/// <summary>
/// Обработчик запроса на получение животного по идентификатору.
/// </summary>
public class GetPetByIdHandler(IPetService petService)
{
    /// <summary>
    /// Обработать запрос на получение животного.
    /// </summary>
    public async Task<PetDto> Handle(GetPetByIdQuery query, CancellationToken ct)
    {
        var pet = await petService.GetPetById(query.VolunteerId, query.PetId, ct);

        var mappedPet = pet.ToDto();

        return mappedPet;
    }
}