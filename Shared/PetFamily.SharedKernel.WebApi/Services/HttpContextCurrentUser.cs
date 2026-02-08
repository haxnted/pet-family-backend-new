using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace PetFamily.SharedKernel.WebApi.Services;

/// <summary>
/// Реализация <see cref="ICurrentUser"/> на основе HTTP-контекста.
/// </summary>
internal sealed class HttpContextCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Создаёт экземпляр HttpContextCurrentUser.
    /// </summary>
    /// <param name="contextAccessor">Accessor для HTTP контекста.</param>
    public HttpContextCurrentUser(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc/>
    public Guid UserId
    {
        get
        {
            var context = _contextAccessor.HttpContext
                          ?? throw new UnauthorizedAccessException("HttpContext недоступен");

            return context.User.GetUserId()
                   ?? throw new UnauthorizedAccessException("Идентификатор пользователя не найден в claims");
        }
    }

    /// <inheritdoc/>
    public Guid? UserIdOrDefault => _contextAccessor.HttpContext?.User.GetUserId();

    /// <inheritdoc/>
    public string? Email => _contextAccessor.HttpContext?.User.GetUserEmail();

    /// <inheritdoc/>
    public string? UserName => _contextAccessor.HttpContext?.User.GetUserName();

    /// <inheritdoc/>
    public bool IsAuthenticated =>
        _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    /// <inheritdoc/>
    public bool HasRole(string role) =>
        _contextAccessor.HttpContext?.User.HasRole(role) == true;

    /// <inheritdoc/>
    public bool IsAdmin =>
        _contextAccessor.HttpContext?.User.IsAdmin() == true;

    /// <inheritdoc/>
    public IEnumerable<string> Roles =>
        _contextAccessor.HttpContext?.User.GetRoles() ?? Enumerable.Empty<string>();
}