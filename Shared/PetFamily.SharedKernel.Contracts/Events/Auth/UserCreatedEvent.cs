using PetFamily.SharedKernel.Contracts.Abstractions;

namespace PetFamily.SharedKernel.Contracts.Events.Auth;

/// <summary>
/// Событие означающее создание пользователя.
/// </summary>
public sealed class UserCreatedEvent(
    Guid userId,
    string email,
    string firstName,
    string lastName,
    string? patronymic,
    string role) : IntegrationEvent
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; } = userId;

    /// <summary>
    /// Почта.
    /// </summary>
    public string Email { get; } = email;

    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; } = firstName;

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; } = lastName;

    /// <summary>
    /// Отчество.
    /// </summary>
    public string? Patronymic { get; } = patronymic;

    /// <summary>
    /// Роль.
    /// </summary>
    public string Role { get; } = role;
}