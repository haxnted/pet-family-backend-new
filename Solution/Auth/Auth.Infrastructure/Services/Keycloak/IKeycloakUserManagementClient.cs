using Auth.Infrastructure.Services.Keycloak.Models;

namespace Auth.Infrastructure.Services.Keycloak;

/// <summary>
/// Клиент для управления пользователями в Keycloak.
/// </summary>
public interface IKeycloakUserManagementClient
{
    /// <summary>
    /// Создает нового пользователя в Keycloak.
    /// </summary>
    /// <param name="request">Запрос на создание пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken ct = default);

    /// <summary>
    /// Назначает роль пользователю.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="roleName">Название роли.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AssignRoleToUserAsync(Guid userId, string roleName, CancellationToken ct = default);

    /// <summary>
    /// Отправляет письмо для подтверждения email.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task SendVerificationEmailAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Отправляет письмо для сброса пароля.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task SendPasswordResetEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Удаляет пользователя из Keycloak.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    Task DeleteUserAsync(Guid userId, CancellationToken ct = default);
}