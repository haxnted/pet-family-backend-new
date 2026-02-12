using Conversation.Domain.Aggregates.Entities;
using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;
using Conversation.Domain.Aggregates.ValueObjects.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conversation.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Message"/>.
/// </summary>
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MessageId.Of(value))
            .ValueGeneratedNever();

        builder.Property(m => m.ChatId)
            .HasConversion(
                id => id.Value,
                value => ChatId.Of(value))
            .IsRequired();

        builder.Property(m => m.Text)
            .HasConversion(
                text => text.Value,
                value => MessageText.Of(value))
            .HasMaxLength(MessageText.MaxLength)
            .IsRequired();

        builder.Property(m => m.UserId)
            .IsRequired();

        builder.Property(m => m.ParentMessageId)
            .HasConversion(
                id => id != null ? id.Value.Value : (Guid?)null,
                value => value.HasValue ? MessageId.Of(value.Value) : null)
            .IsRequired(false);

        builder.HasOne<Message>()
            .WithMany()
            .HasForeignKey(m => m.ParentMessageId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasIndex(m => m.ChatId);
        builder.HasIndex(m => m.ParentMessageId);
    }
}
