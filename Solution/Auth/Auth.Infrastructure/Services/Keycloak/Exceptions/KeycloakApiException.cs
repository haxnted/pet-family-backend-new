namespace Auth.Infrastructure.Services.Keycloak.Exceptions;

/// <summary>
/// Исключение при ошибке вызова Keycloak API.
/// </summary>
/// <param name="message">Сообщение об ошибке.</param>
/// <param name="statusCode">HTTP статус код.</param>
public class KeycloakApiException(string message, int? statusCode = null) : KeycloakException(message)
{
    /// <summary>
    /// HTTP статус код.
    /// </summary>
    public int? StatusCode = statusCode;
}