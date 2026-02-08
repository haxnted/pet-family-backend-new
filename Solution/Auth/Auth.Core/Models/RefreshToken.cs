namespace Auth.Core.Models;

/// <summary>
/// Модель refresh токена.
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Значение токена.
    /// </summary>
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// Дата истечения срока действия.
    /// </summary>
    public DateTime ExpiresAt { get; init; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Отозван ли токен.
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Навигационное свойство к пользователю.
    /// </summary>
    public User User { get; init; } = null!;
}
