namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.RejectAdoption;

/// <summary>
/// Команда на отклонение усыновления.
/// </summary>
/// <param name="SagaId">Идентификатор саги.</param>
/// <param name="CurrentUserId">Keycloak UserId текущего пользователя.</param>
/// <param name="Reason">Причина отказа.</param>
public sealed record RejectAdoptionCommand(Guid SagaId, Guid CurrentUserId, string? Reason);
