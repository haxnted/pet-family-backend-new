namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.ConfirmAdoption;

/// <summary>
/// Команда на подтверждение усыновления.
/// </summary>
/// <param name="SagaId">Идентификатор саги.</param>
/// <param name="CurrentUserId">Keycloak UserId текущего пользователя.</param>
public sealed record ConfirmAdoptionCommand(Guid SagaId, Guid CurrentUserId);
