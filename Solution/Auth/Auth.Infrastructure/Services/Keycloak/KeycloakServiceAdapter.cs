using Auth.Contracts.Dtos;
using Auth.Infrastructure.Services.Keycloak.Models;

namespace Auth.Infrastructure.Services.Keycloak;

/// <summary>
/// Адаптер для обратной совместимости с интерфейсом IKeycloakService.
/// Использует новые клиенты (IKeycloakAuthorizationClient и IKeycloakUserManagementClient)
/// для реализации методов старого интерфейса.
/// </summary>
public class KeycloakServiceAdapter : IKeycloakService
{
    private readonly IKeycloakAuthorizationClient _authClient;
    private readonly IKeycloakUserManagementClient _userClient;

    /// <summary>
    /// Создает экземпляр <see cref="KeycloakServiceAdapter"/>.
    /// </summary>
    /// <param name="authClient">Клиент авторизации.</param>
    /// <param name="userClient">Клиент управления пользователями.</param>
    public KeycloakServiceAdapter(
        IKeycloakAuthorizationClient authClient,
        IKeycloakUserManagementClient userClient)
    {
        _authClient = authClient;
        _userClient = userClient;
    }

    /// <inheritdoc />
    public async Task<Guid> CreateUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string? patronymic,
        CancellationToken ct)
    {
        var request = new CreateUserRequest(email, password, firstName, lastName, patronymic);
        return await _userClient.CreateUserAsync(request, ct);
    }

    /// <inheritdoc />
    public async Task<AuthTokenResponse> LoginAsync(string email, string password, CancellationToken ct)
    {
        var response = await _authClient.LoginAsync(email, password, ct);
        return new AuthTokenResponse(
            response.AccessToken!,
            response.RefreshToken!,
            response.ExpiresIn,
            response.TokenType!);
    }

    /// <inheritdoc />
    public async Task<AuthTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        var response = await _authClient.RefreshTokenAsync(refreshToken, ct);
        return new AuthTokenResponse(
            response.AccessToken!,
            response.RefreshToken!,
            response.ExpiresIn,
            response.TokenType!);
    }

    /// <inheritdoc />
    public Task AssignRoleToUserAsync(Guid userId, string roleName, CancellationToken ct)
        => _userClient.AssignRoleToUserAsync(userId, roleName, ct);

    /// <inheritdoc />
    public Task SendVerificationEmailAsync(Guid userId, CancellationToken ct)
        => _userClient.SendVerificationEmailAsync(userId, ct);

    /// <inheritdoc />
    public Task SendPasswordResetEmailAsync(string email, CancellationToken ct)
        => _userClient.SendPasswordResetEmailAsync(email, ct);

    /// <inheritdoc />
    public Task DeleteUserAsync(Guid userId, CancellationToken ct)
        => _userClient.DeleteUserAsync(userId, ct);
}