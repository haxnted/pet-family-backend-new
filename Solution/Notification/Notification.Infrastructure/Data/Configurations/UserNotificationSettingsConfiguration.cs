using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Core.Models;

namespace Notification.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация для базовых настроек пользователя.
/// </summary>
public class UserNotificationSettingsConfiguration : IEntityTypeConfiguration<UserNotificationSettings>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserNotificationSettings> builder)
    {
        builder.ToTable("user_notification_settings");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.IsMuted).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasOne(x => x.EmailSettings)
            .WithOne(x => x.UserNotificationSettings)
            .HasForeignKey<EmailSettings>(x => x.UserNotificationSettingsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}