using Microsoft.AspNetCore.Http;

namespace PetFamily.SharedKernel.WebApi.Handlers;

/// <summary>
/// DelegatingHandler для проброса Authorization заголовка в исходящие HTTP-запросы.
/// Используется для service-to-service коммуникации с пробросом токена пользователя.
/// </summary>
public sealed class AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
	: DelegatingHandler
{
	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var httpContext = httpContextAccessor.HttpContext;

		if (httpContext?.Request.Headers.TryGetValue("Authorization", out var authHeader) == true)
		{
			request.Headers.TryAddWithoutValidation("Authorization", authHeader.ToString());
		}

		return await base.SendAsync(request, cancellationToken);
	}
}
