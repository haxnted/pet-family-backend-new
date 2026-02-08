using System.Security.Claims;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для работы с ClaimsPrincipal.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Получить идентификатор пользователя из JWT claims.
    /// Проверяет claims в порядке: user_id, sub, NameIdentifier.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>Guid пользователя или null, если не найден.</returns>
    public static Guid? GetUserId(this ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst("user_id")?.Value
                          ?? principal.FindFirst("sub")?.Value
                          ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return null;
        }

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    /// <summary>
    /// Получить идентификатор пользователя из JWT claims.
    /// Выбрасывает исключение, если идентификатор не найден.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>Guid пользователя.</returns>
    /// <exception cref="UnauthorizedAccessException">Если идентификатор пользователя не найден в claims.</exception>
    public static Guid GetRequiredUserId(this ClaimsPrincipal principal)
    {
        return principal.GetUserId()
            ?? throw new UnauthorizedAccessException("Идентификатор пользователя не найден в claims");
    }

    /// <summary>
    /// Получить email пользователя из JWT claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>Email пользователя или null, если не найден.</returns>
    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value
               ?? principal.FindFirst("email")?.Value;
    }

    /// <summary>
    /// Получить имя пользователя из JWT claims.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>Имя пользователя или null, если не найдено.</returns>
    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst("preferred_username")?.Value
               ?? principal.FindFirst(ClaimTypes.Name)?.Value
               ?? principal.FindFirst("name")?.Value;
    }

    /// <summary>
    /// Проверить, имеет ли пользователь указанную роль.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <param name="role">Название роли.</param>
    /// <returns>True, если пользователь имеет роль.</returns>
    public static bool HasRole(this ClaimsPrincipal principal, string role)
    {
        return principal.IsInRole(role);
    }

    /// <summary>
    /// Получить все роли пользователя.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>Коллекция ролей пользователя.</returns>
    public static IEnumerable<string> GetRoles(this ClaimsPrincipal principal)
    {
        return principal.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value);
    }

    /// <summary>
    /// Проверить, является ли пользователь администратором.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>True, если пользователь имеет роль admin.</returns>
    public static bool IsAdmin(this ClaimsPrincipal principal)
    {
        return principal.IsInRole("admin");
    }

    /// <summary>
    /// Проверить, является ли пользователь волонтёром.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>True, если пользователь имеет роль volunteer.</returns>
    public static bool IsVolunteer(this ClaimsPrincipal principal)
    {
        return principal.IsInRole("volunteer");
    }

    /// <summary>
    /// Проверить, аутентифицирован ли пользователь.
    /// </summary>
    /// <param name="principal">ClaimsPrincipal из HttpContext.User.</param>
    /// <returns>True, если пользователь аутентифицирован.</returns>
    public static bool IsAuthenticated(this ClaimsPrincipal principal)
    {
        return principal.Identity?.IsAuthenticated == true;
    }
}
