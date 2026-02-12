using FileStorage.Contracts.Client;
using FileStorage.Contracts.Handlers;
using FileStorage.Contracts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.Contracts.Extensions;

/// <summary>
/// Расширения для регистрации FileStorage клиента в DI-контейнере.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет HTTP-клиент FileStorage и его настройки.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов для цепочки вызовов.</returns>
    public static IServiceCollection AddFileStorageClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<FileStorageSettings>(
            configuration.GetSection(FileStorageSettings.SectionName));

        services.AddHttpContextAccessor();

        services.AddTransient<AuthenticationDelegatingHandler>();

        services.AddHttpClient<IFileStorageClient, FileStorageHttpClient>((sp, client) =>
        {
            var settings = configuration
                .GetSection(FileStorageSettings.SectionName)
                .Get<FileStorageSettings>() ?? new FileStorageSettings();

            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromMinutes(settings.TimeoutMinutes);
        })
        .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return services;
    }
}
