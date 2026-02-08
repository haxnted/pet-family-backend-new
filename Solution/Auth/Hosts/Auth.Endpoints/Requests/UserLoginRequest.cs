namespace Auth.Endpoints.Requests;

/// <summary>
/// Запрос на вход в систему.
/// </summary>
/// <param name="Email">Почта.</param>
/// <param name="Password">Пароль.</param>
public record UserLoginRequest(string Email, string Password);