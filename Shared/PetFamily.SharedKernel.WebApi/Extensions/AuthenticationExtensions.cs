using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки JWT аутентификации с Keycloak.
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Добавляет JWT Bearer аутентификацию с Keycloak.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="mapKeycloakRoles">Включить маппинг ролей из realm_access (по умолчанию true).</param>
    /// <param name="configureOptions">Дополнительная конфигурация JwtBearerOptions.</param>
    /// <returns>AuthenticationBuilder для дальнейшей конфигурации.</returns>
    /// <remarks>
    /// Ожидаемая конфигурация в appsettings.json:
    /// <code>
    /// {
    ///   "Keycloak": {
    ///     "Url": "http://localhost:8080",
    ///     "Realm": "petfamily",
    ///     "Audience": "account" // опционально, по умолчанию "account"
    ///   }
    /// }
    /// </code>
    /// Альтернативно можно указать полный Authority:
    /// <code>
    /// {
    ///   "Keycloak": {
    ///     "Authority": "http://localhost:8080/realms/petfamily"
    ///   }
    /// }
    /// </code>
    /// </remarks>
    public static AuthenticationBuilder AddKeycloakJwtBearer(
        this IServiceCollection services,
        IConfiguration configuration,
        bool mapKeycloakRoles = true,
        Action<JwtBearerOptions>? configureOptions = null)
    {
        var authority = GetAuthority(configuration);
        var audience = configuration["Keycloak:Audience"] ?? "account";

        return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                if (mapKeycloakRoles)
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = OnAuthenticationFailed,
                        OnTokenValidated = OnTokenValidated
                    };
                }

                configureOptions?.Invoke(options);
            });
    }

    /// <summary>
    /// Добавляет стандартные политики авторизации для ролей.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="additionalPolicies">Дополнительные политики (имя -> роли).</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddDefaultAuthorizationPolicies(
        this IServiceCollection services,
        Dictionary<string, string[]>? additionalPolicies = null)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("UserPolicy", policy =>
                policy.RequireRole("user"));

            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("admin"));

            options.AddPolicy("VolunteerPolicy", policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole("user") ||
                    context.User.IsInRole("admin") ||
                    context.User.IsInRole("volunteer")));

            if (additionalPolicies != null)
            {
                foreach (var (policyName, roles) in additionalPolicies)
                {
                    options.AddPolicy(policyName, policy =>
                        policy.RequireRole(roles));
                }
            }
        });

        return services;
    }

    private static string GetAuthority(IConfiguration configuration)
    {
        var authority = configuration["Keycloak:Authority"];
        if (!string.IsNullOrEmpty(authority))
        {
            return authority;
        }

        var keycloakUrl = configuration["Keycloak:Url"]
            ?? throw new InvalidOperationException(
                "Keycloak configuration is missing. Set 'Keycloak:Authority' or 'Keycloak:Url' + 'Keycloak:Realm'");

        var realm = configuration["Keycloak:Realm"]
            ?? throw new InvalidOperationException(
                "Keycloak:Realm is required when using Keycloak:Url");

        return $"{keycloakUrl}/realms/{realm}";
    }

    private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()
            ?.CreateLogger("JwtAuthentication");

        logger?.LogError(context.Exception,
            "JWT Authentication failed: {Message}",
            context.Exception.Message);

        return Task.CompletedTask;
    }

    private static Task OnTokenValidated(TokenValidatedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()
            ?.CreateLogger("JwtAuthentication");

        if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity)
        {
            logger?.LogWarning("ClaimsIdentity is null");
            return Task.CompletedTask;
        }

        var rolesAdded = false;

        // Способ 1: realm_access как claim (JwtSecurityTokenHandler)
        var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;
        if (!string.IsNullOrEmpty(realmAccess))
        {
            rolesAdded = TryAddRolesFromJson(realmAccess, claimsIdentity, logger);
        }

        // Способ 2: из SecurityToken напрямую (JsonWebTokenHandler в .NET 8+)
        if (!rolesAdded && context.SecurityToken is JsonWebToken jwt)
        {
            if (jwt.TryGetPayloadValue<JsonElement>("realm_access", out var realmAccessElement))
            {
                rolesAdded = TryAddRolesFromJsonElement(realmAccessElement, claimsIdentity, logger);
            }
        }

        if (!rolesAdded)
        {
            logger?.LogWarning("realm_access roles not found in JWT token. SecurityToken type: {TokenType}",
                context.SecurityToken?.GetType().Name);
        }

        // sub может быть под разными именами в зависимости от JWT handler
        var sub = context.Principal?.FindFirst("sub")?.Value
                  ?? context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Fallback: читаем из SecurityToken напрямую
        if (string.IsNullOrEmpty(sub) && context.SecurityToken is JsonWebToken jwtForSub)
        {
            jwtForSub.TryGetPayloadValue<string>("sub", out sub);
        }

        if (!string.IsNullOrEmpty(sub))
        {
            claimsIdentity.AddClaim(new Claim("user_id", sub));
        }

        logger?.LogInformation("JWT validated. UserId: {Sub}, Roles: [{Roles}]",
            sub,
            string.Join(", ", claimsIdentity.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)));

        return Task.CompletedTask;
    }

    private static bool TryAddRolesFromJson(string json, ClaimsIdentity identity, ILogger? logger)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return TryAddRolesFromJsonElement(doc.RootElement, identity, logger);
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Failed to parse realm_access JSON string");
            return false;
        }
    }

    private static bool TryAddRolesFromJsonElement(JsonElement element, ClaimsIdentity identity, ILogger? logger)
    {
        try
        {
            if (!element.TryGetProperty("roles", out var rolesElement))
                return false;

            var added = false;
            foreach (var role in rolesElement.EnumerateArray())
            {
                var roleValue = role.GetString();
                if (!string.IsNullOrEmpty(roleValue))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                    added = true;
                }
            }

            return added;
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Failed to parse realm_access element");
            return false;
        }
    }
}
