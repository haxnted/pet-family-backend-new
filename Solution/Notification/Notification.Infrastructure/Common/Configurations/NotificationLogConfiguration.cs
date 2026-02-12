using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Core.Models;

namespace Notification.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация для логов уведомлений.
/// </summary>
public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable("notification_logs");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ExpiresAt);

        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.EventId).IsRequired();

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.ChannelsAttempted).IsRequired();

        builder.Property(x => x.ChannelsSucceeded).IsRequired();

        builder.Property(x => x.Status).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.ExpiresAt).IsRequired();
    }
}