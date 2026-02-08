// using Microsoft.OpenApi;
// using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace ApiGateway;

/// <summary>
/// Dependency injection configuration for API Gateway.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add all program dependencies.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        // Используем SharedKernel.WebApi для аутентификации и CORS
        services.AddKeycloakJwtBearer(configuration, mapKeycloakRoles: false);
        services.AddAuthorization();
        services.AddAllowAllCors("AllowAll");

        services.ConfigureOcelot(configuration);
        // TODO: Включить обратно когда MMLib.SwaggerForOcelot будет поддерживать Microsoft.OpenApi v2
        // services.ConfigureSwagger(configuration);

        services.AddStandardHealthChecks()
            .AddKeycloakHealthCheck(configuration, "Keycloak");

        return services;
    }

    /// <summary>
    /// Configure Ocelot with caching and Polly for QoS.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    private static IServiceCollection ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOcelot(configuration)
            .AddCacheManager(settings =>
            {
                settings.WithDictionaryHandle();
            })
            .AddPolly();

        return services;
    }

    // TODO: Раскомментировать когда MMLib.SwaggerForOcelot будет поддерживать Microsoft.OpenApi v2
    /*
    /// <summary>
    /// Configure Swagger for Ocelot.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    private static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerForOcelot(configuration, options =>
        {
            options.GenerateDocsDocsForGatewayItSelf(opt =>
            {
                opt.GatewayDocsTitle = "Pet Family API Gateway";
                opt.GatewayDocsOpenApiInfo = new OpenApiInfo
                {
                    Title = "Pet Family API Gateway",
                    Version = "v1",
                    Description = "Unified API Gateway for Pet Family microservices"
                };
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token in format: Bearer {token}"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer"),
                        new List<string>()
                    }
                });
            });
        });

        return services;
    }
    */
}
