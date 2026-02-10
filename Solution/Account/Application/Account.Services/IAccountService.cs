using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Services;

/// <summary>
/// Интерфейс-сервис для работы с аккаунтами (профилями пользователей).
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Создать новый аккаунт.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя из Auth.</param>
    /// <param name="ct">Токен отмены.</param>
    Task CreateAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Обновить профильные данные аккаунта.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="phone">Номер телефона.</param>
    /// <param name="experience">Опыт (в годах).</param>
    /// <param name="description">Описание.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateProfileAsync(
        Guid userId,
        string? phone,
        int? experience,
        string? description,
        CancellationToken ct);

    /// <summary>
    /// Обновить фотографию профиля.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="photoId">Идентификатор фотографии (null для удаления).</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdatePhotoAsync(Guid userId, Guid? photoId, CancellationToken ct);

    /// <summary>
    /// Получить аккаунт по идентификатору пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя из Auth.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<DomainAccount> GetByUserIdAsync(Guid userId, CancellationToken ct);
}
