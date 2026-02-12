using Microsoft.AspNetCore.Http;

namespace FileStorage.Contracts.Handlers;

/// <summary>
/// DelegatingHandler для проброса Authorization заголовка в исходящие HTTP запросы.
/// </summary>
public sealed class AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Получаем текущий HttpContext
        var httpContext = httpContextAccessor.HttpContext;

        // Если есть заголовок Authorization в текущем запросе, пробрасываем его дальше
        if (httpContext?.Request.Headers.TryGetValue("Authorization", out var authHeader) == true)
        {
            request.Headers.TryAddWithoutValidation("Authorization", authHeader.ToString());
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
