namespace Auth.Infrastructure.Services.Keycloak.Models;

/// <summary>
/// Представление роли Keycloak.
/// </summary>
/// <param name="Id">Идентификатор роли.</param>
/// <param name="Name">Название роли.</param>
public record KeycloakRoleRepresentation(string Id, string? Name);
