namespace Auth.Contracts.Dtos;

/// <summary>
/// DTO пользователя.
/// </summary>
/// <param name="Id">Идентификатор пользователя.</param>
/// <param name="Email">Почта.</param>
/// <param name="FirstName">Имя.</param>
/// <param name="LastName">Фамилия.</param>
/// <param name="Patronymic">Отчество.</param>
/// <param name="Role">Роль.</param>
/// <param name="EmailVerified">Флаг, означающий подтверждена ли почта.</param>
/// <param name="CreatedAt">Дата создания.</param>
public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Patronymic,
    string Role,
    bool EmailVerified,
    DateTime CreatedAt);