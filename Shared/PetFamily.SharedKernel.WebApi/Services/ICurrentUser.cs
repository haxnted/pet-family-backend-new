using System;
using System.Collections.Generic;

namespace PetFamily.SharedKernel.WebApi.Services;

/// <summary>
/// Контракт для работы с данными текущего пользователя.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Идентификатор пользователя.
    /// Выбрасывает UnauthorizedAccessException, если пользователь не аутентифицирован.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Идентификатор пользователя или null, если не аутентифицирован.
    /// </summary>
    Guid? UserIdOrDefault { get; }

    /// <summary>
    /// Email пользователя или null.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Имя пользователя или null.
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Возвращает true, если пользователь аутентифицирован.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Проверить, имеет ли пользователь указанную роль.
    /// </summary>
    /// <param name="role">Название роли.</param>
    /// <returns>True, если пользователь имеет роль.</returns>
    bool HasRole(string role);

    /// <summary>
    /// Возвращает true, если пользователь является администратором.
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Получить все роли пользователя.
    /// </summary>
    IEnumerable<string> Roles { get; }
}