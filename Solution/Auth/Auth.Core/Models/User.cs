namespace Auth.Core.Models;

/// <summary>
/// Модель пользователя в локальной БД.
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор (совпадает с Keycloak User ID).
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Отчество.
    /// </summary>
    public string? Patronymic { get; init; }

    /// <summary>
    /// Роль пользователя (user, volunteer, admin).
    /// </summary>
    public string Role { get; init; } = "user";

    /// <summary>
    /// Подтвержден ли email.
    /// </summary>
    public bool EmailVerified { get; init; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Коллекция refresh токенов.
    /// </summary>
    public List<RefreshToken> RefreshTokens { get; init; } = [];
}
