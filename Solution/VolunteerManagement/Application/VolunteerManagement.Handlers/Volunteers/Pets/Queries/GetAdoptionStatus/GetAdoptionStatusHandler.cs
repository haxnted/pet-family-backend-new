using VolunteerManagement.Services.Volunteers.Adoption;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetAdoptionStatus;

/// <summary>
/// Обработчик запроса на получение статуса усыновления.
/// </summary>
public class GetAdoptionStatusHandler(IPetAdoptionService adoptionService)
{
    /// <summary>
    /// Обработать запрос.
    /// </summary>
    public async Task<AdoptionStatusDto?> Handle(GetAdoptionStatusQuery query, CancellationToken ct)
    {
        return await adoptionService.GetStatusAsync(query.SagaId, ct);
    }
}
