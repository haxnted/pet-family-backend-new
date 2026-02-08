namespace PetFamily.SharedKernel.Infrastructure.Options;

/// <summary>
/// Настройки для интеграции с Keycloak.
/// </summary>
public class KeycloakOptions
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public const string SectionName = "Keycloak";

    /// <summary>
    /// URL Keycloak сервера (например: http://localhost:8080).
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Имя Realm.
    /// </summary>
    public string Realm { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Секрет клиента.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Authority URL (вычисляемое свойство).
    /// </summary>
    public string Authority => $"{Url}/realms/{Realm}";

    /// <summary>
    /// Адрес метаданных OpenID Connect (вычисляемое свойство).
    /// </summary>
    public string MetadataAddress => $"{Authority}/.well-known/openid-configuration";

    /// <summary>
    /// Endpoint для получения токенов (вычисляемое свойство).
    /// </summary>
    public string TokenEndpoint => $"{Authority}/protocol/openid-connect/token";

    /// <summary>
    /// Базовый URL для Admin API (вычисляемое свойство).
    /// </summary>
    public string AdminApiBaseUrl => $"{Url}/admin/realms/{Realm}/";
}