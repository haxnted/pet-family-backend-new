using Auth.Contracts.Dtos;

namespace Auth.Infrastructure.Services;

/// <summary>
/// Сервис для взаимодействия с Keycloak.
/// </summary>
public interface IKeycloakService
{
    /// <summary>
    /// Создать пользователя в Keycloak.
    /// </summary>
    Task<Guid> CreateUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string? patronymic,
        CancellationToken ct);

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    Task<AuthTokenResponse> LoginAsync(string email, string password, CancellationToken ct);

    /// <summary>
    /// Обновить access token используя refresh token.
    /// </summary>
    Task<AuthTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct);

    /// <summary>
    /// Назначить роль пользователю.
    /// </summary>
    Task AssignRoleToUserAsync(Guid userId, string roleName, CancellationToken ct);

    /// <summary>
    /// Отправить письмо для подтверждения email.
    /// </summary>
    Task SendVerificationEmailAsync(Guid userId, CancellationToken ct);

    /// <summary>
    /// Отправить письмо для восстановления пароля.
    /// </summary>
    Task SendPasswordResetEmailAsync(string email, CancellationToken ct);

    /// <summary>
    /// Удалить пользователя из Keycloak.
    /// </summary>
    Task DeleteUserAsync(Guid userId, CancellationToken ct);
}