using FileStorage.Application.Services;
using FileStorage.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.Application;

/// <summary>
/// Регистрация зависимостей Application слоя
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости из Application слоя
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddValidatorsFromAssemblyContaining<FormFileValidator>();

        return services;
    }
}
