using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.WebApi.Extensions;
using PetFamily.SharedKernel.WebApi.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetFamily.SharedKernel.WebApi;

/// <summary>
/// Агрегированные методы для быстрой настройки WebApi сервисов.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет все стандартные WebApi сервисы: Swagger, JWT Auth, CORS, CurrentUser.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="apiTitle">Заголовок API для Swagger.</param>
    /// <param name="apiDescription">Описание API для Swagger.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddSharedWebApi(
        this IServiceCollection services,
        IConfiguration configuration,
        string apiTitle,
        string? apiDescription = null)
    {
        services.AddSwaggerGenWithJwt(apiTitle, apiDescription);
        services.AddKeycloakJwtBearer(configuration);
        services.AddDefaultAuthorizationPolicies();
        services.AddAllowAllCors();
        services.AddCurrentUser();

        return services;
    }

    /// <summary>
    /// Добавляет все стандартные WebApi сервисы с MassTransit.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="apiTitle">Заголовок API для Swagger.</param>
    /// <param name="configureBus">Конфигурация MassTransit consumers.</param>
    /// <param name="configureRabbitMq">Расширенная конфигурация RabbitMQ.</param>
    /// <param name="apiDescription">Описание API для Swagger.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddSharedWebApiWithMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        string apiTitle,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureRabbitMq = null,
        string? apiDescription = null)
    {
        services.AddSharedWebApi(configuration, apiTitle, apiDescription);
        services.AddMassTransitWithRabbitMq(configuration, configureBus, configureRabbitMq);

        return services;
    }

    /// <summary>
    /// Добавляет Swagger с JWT и Keycloak аутентификацию (без MassTransit).
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="apiTitle">Заголовок API для Swagger.</param>
    /// <param name="configureSwagger">Дополнительная конфигурация Swagger.</param>
    /// <param name="configureJwt">Дополнительная конфигурация JWT.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddSwaggerAndAuth(
        this IServiceCollection services,
        IConfiguration configuration,
        string apiTitle,
        Action<SwaggerGenOptions>? configureSwagger = null,
        Action<JwtBearerOptions>? configureJwt = null)
    {
        services.AddSwaggerGenWithJwt(apiTitle, configureOptions: configureSwagger);
        services.AddKeycloakJwtBearer(configuration, configureOptions: configureJwt);
        services.AddDefaultAuthorizationPolicies();

        return services;
    }
}