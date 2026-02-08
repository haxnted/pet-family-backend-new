namespace Auth.Infrastructure.Services.Keycloak.Models;

/// <summary>
/// Запрос на создание пользователя в Keycloak.
/// </summary>
/// <param name="Email">Email пользователя.</param>
/// <param name="Password">Пароль.</param>
/// <param name="FirstName">Имя.</param>
/// <param name="LastName">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Enabled">Признак активности пользователя.</param>
/// <param name="EmailVerified">Признак подтверждения email.</param>
public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Patronymic,
    bool Enabled = true,
    bool EmailVerified = true);
