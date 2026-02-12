using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using PetFamily.SharedKernel.WebApi.Extensions.SwaggerFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для настройки Swagger с JWT Bearer аутентификацией.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Добавляет Swagger с поддержкой JWT Bearer аутентификации.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="title">Заголовок API.</param>
    /// <param name="description">Описание API.</param>
    /// <param name="version">Версия API (по умолчанию "v1").</param>
    /// <param name="configureOptions">Дополнительная конфигурация SwaggerGen.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddSwaggerGenWithJwt(
        this IServiceCollection services,
        string title,
        string? description = null,
        string version = "v1",
        Action<SwaggerGenOptions>? configureOptions = null)
    {
        services.AddSwaggerGen(options =>
        {
            // Явно указываем версию OpenAPI спецификации для Swashbuckle 10.x
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version,
                Description = description
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен (без префикса Bearer)"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });

            options.DocumentFilter<HealthChecksDocumentFilter>();
            options.OperationFilter<FileUploadOperationFilter>();

            // Настройка для Swashbuckle 10.x: явно указываем OpenAPI 3.0
            options.CustomSchemaIds(type => type.FullName);

            configureOptions?.Invoke(options);
        });

        return services;
    }
}