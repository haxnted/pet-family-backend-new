namespace Auth.Contracts.Dtos;

/// <summary>
/// Ответ с токенами доступа.
/// </summary>
/// <param name="AccessToken">Access токен.</param>
/// <param name="RefreshToken">Refresh токен.</param>
/// <param name="ExpiresIn">Время истечения токена в секундах.</param>
/// <param name="TokenType">Тип токена.</param>
public record AuthTokenResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string TokenType = "Bearer");