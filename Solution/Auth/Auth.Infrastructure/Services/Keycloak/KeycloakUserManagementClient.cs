using System.Net.Http.Json;
using Auth.Infrastructure.Services.Keycloak.Exceptions;
using Auth.Infrastructure.Services.Keycloak.Models;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Infrastructure.Options;

namespace Auth.Infrastructure.Services.Keycloak;

/// <summary>
/// Реализация клиента для управления пользователями в Keycloak.
/// </summary>
public class KeycloakUserManagementClient : IKeycloakUserManagementClient
{
    private readonly HttpClient _httpClient;
    private static readonly string[] InputValue = ["UPDATE_PASSWORD"];

    /// <summary>
    /// Создает экземпляр <see cref="KeycloakUserManagementClient"/>.
    /// </summary>
    /// <param name="httpClient">HTTP клиент с настроенной авторизацией.</param>
    /// <param name="options">Настройки Keycloak.</param>
    public KeycloakUserManagementClient(
        HttpClient httpClient,
        IOptions<KeycloakOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.AdminApiBaseUrl);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var content = JsonContent.Create(new
        {
            username = request.Email,
            email = request.Email,
            firstName = request.FirstName,
            lastName = request.LastName,
            enabled = request.Enabled,
            emailVerified = request.EmailVerified,
            credentials = new[]
            {
                new
                {
                    type = "password",
                    value = request.Password,
                    temporary = false
                }
            },
            attributes = string.IsNullOrEmpty(request.Patronymic)
                ? null
                : new Dictionary<string, string[]> { { "patronymic", [request.Patronymic] } }
        });

        var response = await _httpClient.PostAsync("users", content, ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка создания пользователя: {errorContent}",
                (int)response.StatusCode);
        }

        var locationHeader = response.Headers.Location?.ToString();
        if (string.IsNullOrEmpty(locationHeader))
        {
            throw new KeycloakException("Ошибка получения идентификатора пользователя");
        }

        var userIdString = locationHeader.Split('/').Last();
        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new KeycloakException($"Неверный идентификатор пользователя: {userIdString}");
        }

        return userId;
    }

    /// <inheritdoc />
    public async Task AssignRoleToUserAsync(Guid userId, string roleName, CancellationToken ct = default)
    {
        var getRoleResponse = await _httpClient.GetAsync($"roles/{roleName}", ct);

        if (!getRoleResponse.IsSuccessStatusCode)
        {
            var errorContent = await getRoleResponse.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка получения роли '{roleName}': {errorContent}",
                (int)getRoleResponse.StatusCode);
        }

        var role = await getRoleResponse.Content.ReadFromJsonAsync<KeycloakRoleRepresentation>(ct);

        if (role == null || string.IsNullOrEmpty(role.Id))
        {
            throw new KeycloakException($"Роль '{roleName}' не найдена");
        }

        var content = JsonContent.Create(new[]
        {
            new { id = role.Id, name = role.Name }
        });

        var assignResponse = await _httpClient.PostAsync(
            $"users/{userId}/role-mappings/realm",
            content,
            ct);

        if (!assignResponse.IsSuccessStatusCode)
        {
            var errorContent = await assignResponse.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка назначения роли: {errorContent}",
                (int)assignResponse.StatusCode);
        }
    }

    /// <inheritdoc />
    public async Task SendVerificationEmailAsync(Guid userId, CancellationToken ct = default)
    {
        var response = await _httpClient.PutAsync(
            $"users/{userId}/send-verify-email",
            null,
            ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка при отправке сообщения на почту: {errorContent}",
                (int)response.StatusCode);
        }
    }

    /// <inheritdoc />
    public async Task SendPasswordResetEmailAsync(string email, CancellationToken ct = default)
    {
        var searchResponse = await _httpClient.GetAsync($"users?email={Uri.EscapeDataString(email)}", ct);

        if (!searchResponse.IsSuccessStatusCode)
        {
            var errorContent = await searchResponse.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Пользователь не найден: {errorContent}",
                (int)searchResponse.StatusCode);
        }

        var users = await searchResponse.Content.ReadFromJsonAsync<KeycloakUserRepresentation[]>(ct);

        if (users == null || users.Length == 0)
        {
            throw new KeycloakUserNotFoundException($"Пользователь с почтой '{email}' не найден");
        }

        var userId = users[0].Id;

        var content = JsonContent.Create(InputValue);
        var resetResponse = await _httpClient.PutAsync(
            $"users/{userId}/execute-actions-email",
            content,
            ct);

        if (!resetResponse.IsSuccessStatusCode)
        {
            var errorContent = await resetResponse.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка при отправке ссылки на сброс пароля: {errorContent}",
                (int)resetResponse.StatusCode);
        }
    }

    /// <inheritdoc />
    public async Task DeleteUserAsync(Guid userId, CancellationToken ct = default)
    {
        var response = await _httpClient.DeleteAsync($"users/{userId}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(ct);
            throw new KeycloakApiException(
                $"Ошибка при удалении пользователя: {errorContent}",
                (int)response.StatusCode);
        }
    }
}