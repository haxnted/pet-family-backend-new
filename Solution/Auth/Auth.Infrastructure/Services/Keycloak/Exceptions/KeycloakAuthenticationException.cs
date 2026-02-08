namespace Auth.Infrastructure.Services.Keycloak.Exceptions;

/// <summary>
/// Исключение при ошибке аутентификации в Keycloak.
/// </summary>
/// <param name="message">Сообщение об ошибке.</param>
public class KeycloakAuthenticationException(string message) : KeycloakException(message);