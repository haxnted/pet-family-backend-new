using Auth.Contracts.Dtos;

namespace Auth.Application.Services;

/// <summary>
/// Сервис аутентификации и авторизации.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="email">Почта.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="firstName">Имя.</param>
    /// <param name="lastName">Фамилия.</param>
    /// <param name="patronymic">Отчество.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string? patronymic,
        CancellationToken ct);

    /// <summary>
    /// Вход в систему.
    /// </summary>
    /// <param name="email">Почта.</param>
    /// <param name="password">Пароль.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<AuthTokenResponse> LoginAsync(
        string email,
        string password,
        CancellationToken ct);

    /// <summary>
    /// Обновление токена доступа.
    /// </summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<AuthTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct);

    /// <summary>
    /// Повторная отправка письма для подтверждения email.
    /// </summary>
    Task ResendVerificationEmailAsync(string email, CancellationToken ct);

    /// <summary>
    /// Отправка письма для восстановления пароля.
    /// </summary>
    Task ForgotPasswordAsync(string email, CancellationToken ct);

    /// <summary>
    /// Получить информацию о текущем пользователе.
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken ct);
}