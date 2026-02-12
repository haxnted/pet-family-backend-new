using FileStorage.Application;
using FileStorage.Application.Services;
using FileStorage.Consumers.Consumers;
using FileStorage.Infrastructure.MinIo;
using FileStorage.Infrastructure.Settings;
using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.FileStorage;
using PetFamily.SharedKernel.Infrastructure.Options;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace FileStorage.Consumers;

/// <summary>
/// Класс для настройки зависимостей для worker сервиса FileStorage.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Настройка зависимостей worker сервиса.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static void AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.SectionName));

        services.Configure<MinIoSettings>(configuration.GetSection(MinIoSettings.SectionName));

        services.AddApplication();

        services.AddSingleton<IMinIoService, MinIoService>();

        services.AddMassTransitWithRabbitMq(
            configuration,
            configureBus: (cfg) => cfg.AddConsumer<FileDeleteRequestedConsumer>(),
            configureRabbitMq: (context, cfg) =>
            {
                cfg.Message<FileDeleteRequestedEvent>(e => e.SetEntityName("filestorage-events"));

                cfg.ReceiveEndpoint("filestorage-delete-requests", e =>
                {
                    e.Bind("filestorage-events");
                    e.ConfigureConsumer<FileDeleteRequestedConsumer>(context);

                    e.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
                });
            });
    }
}