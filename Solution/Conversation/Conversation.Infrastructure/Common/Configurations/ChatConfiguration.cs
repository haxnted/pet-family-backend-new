using Conversation.Domain.Aggregates;
using Conversation.Domain.Aggregates.ValueObjects;
using Conversation.Domain.Aggregates.ValueObjects.Identifiers;
using Conversation.Domain.Aggregates.ValueObjects.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conversation.Infrastructure.Common.Configurations;

/// <summary>
/// Конфигурация агрегата <see cref="Chat"/>.
/// </summary>
public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ChatId.Of(value))
            .ValueGeneratedNever();

        builder.Property(c => c.Title)
            .HasConversion(
                title => title.Value,
                value => Title.Of(value))
            .HasMaxLength(Title.MaxLength)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasConversion(
                desc => desc != null ? desc.Value : null,
                value => value != null ? Description.Of(value) : null)
            .HasMaxLength(Description.MaxLength)
            .IsRequired(false);

        builder.Property(c => c.LinkedId)
            .IsRequired();

        builder.HasIndex(c => c.LinkedId);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
