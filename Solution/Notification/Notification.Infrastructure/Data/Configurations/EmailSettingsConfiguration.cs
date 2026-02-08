using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Core.Models;

namespace Notification.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация настройки почты пользователя.
/// </summary>
public class EmailSettingsConfiguration : IEntityTypeConfiguration<EmailSettings>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<EmailSettings> builder)
    {
        builder.ToTable("email_settings");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserNotificationSettingsId).IsUnique();

        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);

        builder.Property(x => x.IsEnabled).IsRequired().HasDefaultValue(true);
    }
}