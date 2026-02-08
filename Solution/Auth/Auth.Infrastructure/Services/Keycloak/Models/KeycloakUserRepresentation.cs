namespace Auth.Infrastructure.Services.Keycloak.Models;

/// <summary>
/// Представление пользователя Keycloak.
/// </summary>
public class KeycloakUserRepresentation
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Имя пользователя (username).
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Имя.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Признак активности пользователя.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Признак подтверждения email.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Дополнительные атрибуты пользователя.
    /// </summary>
    public Dictionary<string, string[]>? Attributes { get; set; }
}
