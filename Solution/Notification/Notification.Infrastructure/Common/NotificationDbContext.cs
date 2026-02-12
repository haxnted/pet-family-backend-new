using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Core.Models;

namespace Notification.Infrastructure.Common;

/// <summary>
/// Контекст для работы с уведомлениями.
/// </summary>
public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Базовые настройки уведомлений пользователей.
    /// </summary>
    public DbSet<UserNotificationSettings> UserNotificationSettings => Set<UserNotificationSettings>();

    /// <summary>
    /// Настройки уведомлений для почты.
    /// </summary>
    public DbSet<EmailSettings> EmailSettings => Set<EmailSettings>();

    /// <summary>
    /// Логи доставки уведомлений.
    /// </summary>
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomModelBuilder.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}