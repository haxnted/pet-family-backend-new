namespace VolunteerManagement.Handlers.Volunteers.Pets.Queries.GetAdoptionStatus;

/// <summary>
/// Запрос на получение статуса усыновления.
/// </summary>
/// <param name="SagaId">Идентификатор саги.</param>
public sealed record GetAdoptionStatusQuery(Guid SagaId);
