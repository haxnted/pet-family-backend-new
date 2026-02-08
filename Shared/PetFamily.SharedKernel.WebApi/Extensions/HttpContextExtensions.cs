using Microsoft.AspNetCore.Http;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для работы с HttpContext.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Получить идентификатор текущего пользователя из HttpContext.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>Guid пользователя или null, если не найден.</returns>
    public static Guid? GetCurrentUserId(this HttpContext context)
    {
        return context.User.GetUserId();
    }

    /// <summary>
    /// Получить идентификатор текущего пользователя из HttpContext.
    /// Выбрасывает исключение, если идентификатор не найден.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>Guid пользователя.</returns>
    /// <exception cref="UnauthorizedAccessException">Если идентификатор пользователя не найден.</exception>
    public static Guid GetRequiredCurrentUserId(this HttpContext context)
    {
        return context.User.GetRequiredUserId();
    }

    /// <summary>
    /// Получить email текущего пользователя из HttpContext.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>Email пользователя или null, если не найден.</returns>
    public static string? GetCurrentUserEmail(this HttpContext context)
    {
        return context.User.GetUserEmail();
    }

    /// <summary>
    /// Получить имя текущего пользователя из HttpContext.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>Имя пользователя или null, если не найдено.</returns>
    public static string? GetCurrentUserName(this HttpContext context)
    {
        return context.User.GetUserName();
    }

    /// <summary>
    /// Проверить, является ли текущий пользователь администратором.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>True, если пользователь является администратором.</returns>
    public static bool IsCurrentUserAdmin(this HttpContext context)
    {
        return context.User.IsAdmin();
    }

    /// <summary>
    /// Проверить, аутентифицирован ли текущий пользователь.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>True, если пользователь аутентифицирован.</returns>
    public static bool IsAuthenticated(this HttpContext context)
    {
        return context.User.IsAuthenticated();
    }

    /// <summary>
    /// Получить все роли текущего пользователя.
    /// </summary>
    /// <param name="context">HTTP контекст.</param>
    /// <returns>Коллекция ролей пользователя.</returns>
    public static IEnumerable<string> GetCurrentUserRoles(this HttpContext context)
    {
        return context.User.GetRoles();
    }
}