using MassTransit;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Infrastructure.Common.Contexts;
using VolunteerManagement.Infrastructure.SagaStates;
using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Services.Volunteers.Adoption;

/// <summary>
/// Реализация сервиса для работы с процессом усыновления.
/// </summary>
public class PetAdoptionService(
    VolunteerManagementDbContext dbContext,
    IBus massTransitBus) : IPetAdoptionService
{
    /// <inheritdoc />
    public async Task<AdoptionStatusDto?> GetStatusAsync(Guid sagaId, CancellationToken ct)
    {
        var state = await dbContext.PetAdoptionStates
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.CorrelationId == sagaId, ct);

        return state?.ToDto();
    }

    /// <inheritdoc />
    public async Task ConfirmAsync(Guid sagaId, Guid currentUserId, CancellationToken ct)
    {
        var state = await GetRequiredStateAsync(sagaId, ct);

        ValidateState(state, "WaitingForAdoption", "подтвердить");

        var volunteerId = await GetVolunteerIdByUserIdAsync(currentUserId, ct);

        if (volunteerId != state.VolunteerId)
            throw new ForbiddenException("Только волонтёр-владелец питомца может подтвердить усыновление.");

        await massTransitBus.Publish(new ConfirmAdoption
        {
            CorrelationId = sagaId,
            VolunteerId = state.VolunteerId
        }, ct);
    }

    /// <inheritdoc />
    public async Task RejectAsync(Guid sagaId, Guid currentUserId, string? reason, CancellationToken ct)
    {
        var state = await GetRequiredStateAsync(sagaId, ct);

        ValidateState(state, "WaitingForAdoption", "отклонить");

        var volunteerId = await GetVolunteerIdByUserIdAsync(currentUserId, ct);

        if (volunteerId != state.VolunteerId)
            throw new ForbiddenException("Только волонтёр-владелец питомца может отклонить усыновление.");

        await massTransitBus.Publish(new RejectAdoption
        {
            CorrelationId = sagaId,
            VolunteerId = state.VolunteerId,
            Reason = reason
        }, ct);
    }

    /// <summary>
    /// Получить состояние саги или выбросить исключение, если не найдено.
    /// </summary>
    /// <param name="sagaId">Идентификатор саги.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Состояние саги.</returns>
    /// <exception cref="EntityNotFoundException{PetAdoptionState}">Если сага не найдена.</exception>
    private async Task<PetAdoptionState> GetRequiredStateAsync(Guid sagaId, CancellationToken ct)
    {
        return await dbContext.PetAdoptionStates
                   .AsNoTracking()
                   .FirstOrDefaultAsync(s => s.CorrelationId == sagaId, ct)
               ?? throw new EntityNotFoundException<PetAdoptionState>(sagaId);
    }

    /// <summary>
    /// Проверить, что сага находится в ожидаемом состоянии.
    /// </summary>
    /// <param name="state">Состояние саги.</param>
    /// <param name="expectedState">Ожидаемое состояние.</param>
    /// <param name="action">Описание действия для сообщения об ошибке.</param>
    /// <exception cref="InvalidOperationException">Если состояние не соответствует ожидаемому.</exception>
    private static void ValidateState(PetAdoptionState state, string expectedState, string action)
    {
        if (state.CurrentState != expectedState)
            throw new InvalidOperationException(
                $"Невозможно {action} усыновление в состоянии '{state.CurrentState}'.");
    }

    /// <summary>
    /// Получить доменный идентификатор волонтёра по Keycloak UserId.
    /// </summary>
    /// <param name="keycloakUserId">Идентификатор пользователя из Keycloak (sub claim).</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Доменный идентификатор волонтёра.</returns>
    /// <exception cref="EntityNotFoundException{Volunteer}">Если волонтёр не найден.</exception>
    private async Task<Guid> GetVolunteerIdByUserIdAsync(Guid keycloakUserId, CancellationToken ct)
    {
        var volunteer = await dbContext.Volunteers
                            .AsNoTracking()
                            .FirstOrDefaultAsync(v => v.UserId == keycloakUserId, ct)
                        ?? throw new EntityNotFoundException<Volunteer>(keycloakUserId);

        return volunteer.Id.Value;
    }
}