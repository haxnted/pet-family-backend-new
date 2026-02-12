using VolunteerManagement.Services.Volunteers.Dtos;

namespace VolunteerManagement.Services.Volunteers.Adoption;

/// <summary>
/// Сервис для работы с процессом усыновления питомцев.
/// </summary>
public interface IPetAdoptionService
{
    /// <summary>
    /// Получить статус процесса усыновления по идентификатору саги.
    /// </summary>
    Task<AdoptionStatusDto?> GetStatusAsync(Guid sagaId, CancellationToken ct);

    /// <summary>
    /// Подтвердить усыновление. Проверяет состояние саги и права волонтёра.
    /// </summary>
    /// <param name="sagaId">Идентификатор саги.</param>
    /// <param name="currentUserId">Keycloak UserId текущего пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task ConfirmAsync(Guid sagaId, Guid currentUserId, CancellationToken ct);

    /// <summary>
    /// Отклонить усыновление. Проверяет состояние саги и права волонтёра.
    /// </summary>
    /// <param name="sagaId">Идентификатор саги.</param>
    /// <param name="currentUserId">Keycloak UserId текущего пользователя.</param>
    /// <param name="reason">Причина отказа.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RejectAsync(Guid sagaId, Guid currentUserId, string? reason, CancellationToken ct);
}
