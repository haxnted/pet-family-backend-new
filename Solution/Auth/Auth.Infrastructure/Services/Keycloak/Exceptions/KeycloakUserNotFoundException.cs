namespace Auth.Infrastructure.Services.Keycloak.Exceptions;

/// <summary>
/// Исключение когда пользователь не найден в Keycloak.
/// </summary>
/// <param name="message">Сообщение об ошибке.</param>
public class KeycloakUserNotFoundException(string message) : KeycloakException(message);