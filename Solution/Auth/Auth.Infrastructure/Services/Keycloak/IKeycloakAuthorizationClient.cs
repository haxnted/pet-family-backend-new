using IdentityModel.Client;

namespace Auth.Infrastructure.Services.Keycloak;

/// <summary>
/// Клиент для операций авторизации в Keycloak.
/// </summary>
public interface IKeycloakAuthorizationClient
{
    /// <summary>
    /// Выполняет вход пользователя.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Токены доступа.</returns>
    Task<TokenResponse> LoginAsync(string email, string password, CancellationToken ct = default);

    /// <summary>
    /// Обновляет токен доступа используя refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Новые токены доступа.</returns>
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
}