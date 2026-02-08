namespace Auth.Endpoints.Requests;

/// <summary>
/// Запрос на регистрацию пользователя.
/// </summary>
/// <param name="Email">Почта.</param>
/// <param name="Password">Пароль.</param>
/// <param name="FirstName">Имя.</param>
/// <param name="LastName">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
public record UserRegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Patronymic);