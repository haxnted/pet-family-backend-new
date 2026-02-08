using Auth.Infrastructure.Services.Keycloak.Exceptions;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using PetFamily.SharedKernel.Infrastructure.Options;

namespace Auth.Infrastructure.Services.Keycloak;

/// <summary>
/// Реализация клиента для операций авторизации в Keycloak.
/// </summary>
public class KeycloakAuthorizationClient : IKeycloakAuthorizationClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly KeycloakOptions _options;

    /// <summary>
    /// Создает экземпляр <see cref="KeycloakAuthorizationClient"/>.
    /// </summary>
    /// <param name="httpClientFactory">Фабрика HTTP клиентов.</param>
    /// <param name="options">Настройки Keycloak.</param>
    public KeycloakAuthorizationClient(
        IHttpClientFactory httpClientFactory,
        IOptions<KeycloakOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<TokenResponse> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("Keycloak");

        var disco = await GetDiscoveryAsync(httpClient, ct);

        var request = new PasswordTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            UserName = email,
            Password = password,
            Scope = "openid"
        };

        var response = await httpClient.RequestPasswordTokenAsync(request, ct);

        if (!response.IsError) return response;

        if (response.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new KeycloakAuthenticationException("Неверный адрес электронной почты или пароль");
        }

        throw new KeycloakException($"Ошибка логина: {response.Error} - {response.ErrorDescription}");
    }

    /// <inheritdoc />
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("Keycloak");

        var disco = await GetDiscoveryAsync(httpClient, ct);

        var request = new RefreshTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            RefreshToken = refreshToken
        };

        var response = await httpClient.RequestRefreshTokenAsync(request, ct);

        if (response.IsError)
        {
            throw new KeycloakException(
                $"Ошибка при получении refresh-token: {response.Error} - {response.ErrorDescription}");
        }

        return response;
    }

    /// <summary>
    /// Получить discovery-документ.
    /// </summary>
    /// <param name="httpClient">Http клиент.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <exception cref="KeycloakException">
    /// Если не получилось получить discovery-документ.
    /// </exception>
    private async Task<DiscoveryDocumentResponse> GetDiscoveryAsync(
        HttpClient httpClient,
        CancellationToken ct)
    {
        var disco = await httpClient.GetDiscoveryDocumentAsync(
            new DiscoveryDocumentRequest
            {
                Address = _options.MetadataAddress,
                Policy =
                {
                    RequireHttps = false,
                    ValidateIssuerName = false
                }
            },
            ct);

        if (disco.IsError)
        {
            throw new KeycloakException($"Не удалось получить discovery-документ Keycloak: {disco.Error}");
        }

        return disco;
    }
}