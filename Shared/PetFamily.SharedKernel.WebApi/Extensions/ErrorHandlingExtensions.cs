using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.SharedKernel.WebApi.Errors;

namespace PetFamily.SharedKernel.WebApi.Extensions;

/// <summary>
/// Extension методы для регистрации обработки ошибок
/// </summary>
public static class ErrorHandlingExtensions
{
    /// <summary>
    /// Добавляет глобальную обработку ошибок
    /// </summary>
    public static IServiceCollection AddGlobalErrorHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    /// <summary>
    /// Использует глобальную обработку ошибок
    /// </summary>
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();

        return app;
    }
}