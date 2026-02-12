using VolunteerManagement.Infrastructure.SagaStates;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Services.Volunteers.Adoption;

/// <summary>
/// Методы расширения для маппинга <see cref="PetAdoptionState"/> в <see cref="AdoptionStatusDto"/> DTO.
/// </summary>
public static class AdoptionStateMappingExtensions
{
    /// <summary>
    /// Преобразовать состояние саги <see cref="PetAdoptionState"/> в <see cref="AdoptionStatusDto"/> DTO.
    /// </summary>
    /// <param name="state">Состояние саги усыновления.</param>
    /// <returns>DTO статуса усыновления.</returns>
    public static AdoptionStatusDto ToDto(this PetAdoptionState state)
    {
        return new AdoptionStatusDto(
            state.CorrelationId,
            state.CurrentState,
            state.PetId,
            state.VolunteerId,
            state.AdopterId,
            state.AdopterName,
            state.PetNickName,
            state.ChatId,
            state.CreatedAt,
            state.UpdatedAt,
            state.FailureReason);
    }
}
