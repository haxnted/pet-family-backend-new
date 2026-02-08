using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Core.Models;
using Notification.Infrastructure.BackgroundJobs;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Services.Email;
using Notification.Infrastructure.Settings;
using PetFamily.SharedKernel.Infrastructure;
using PetFamily.SharedKernel.Infrastructure.Abstractions;

namespace Notification.Infrastructure;

/// <summary>
/// Регистрация сервисов Infrastructure слоя.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить все зависимости Infrastructure слоя.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("NotificationDbContext")));

        services.AddScoped<IMigrator, NotificationMigrator>();

        services.AddScoped<IRepository<UserNotificationSettings>>(sp =>
            new EntityFrameworkRepository<UserNotificationSettings>(sp.GetRequiredService<NotificationDbContext>()));

        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SectionName));
        services.AddScoped<IEmailService, EmailService>();

        services.AddHostedService<NotificationLogCleanupService>();

        return services;
    }
}