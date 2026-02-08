namespace Auth.Infrastructure.Services.Keycloak.Exceptions;

/// <summary>
/// Базовое исключение для ошибок Keycloak.
/// </summary>
/// <param name="message">Сообщение об ошибке.</param>
public class KeycloakException(string message) : Exception(message);