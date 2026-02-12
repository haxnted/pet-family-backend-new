using Microsoft.EntityFrameworkCore;
using Notification.Infrastructure.Common.Configurations;
using PetFamily.SharedKernel.Infrastructure.Configurations;

namespace Notification.Infrastructure.Common;

/// <summary>
/// Сборщик для контекста <see cref="NotificationDbContext"/>.
/// </summary>
internal abstract class CustomModelBuilder
{
    /// <summary>
    /// Собирает контекст конфигуратора EF для <see cref="NotificationDbContext"/>.
    /// </summary>
    /// <param name="modelBuilder">Конфигуратор модели.</param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserNotificationSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationLogConfiguration());
        modelBuilder.ApplyConfiguration(new EmailSettingsConfiguration());

        modelBuilder.SetDefaultDateTimeKind(DateTimeKind.Utc);
    }
}