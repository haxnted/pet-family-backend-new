using Conversation.Domain.Aggregates;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Conversation.Infrastructure.Common.Contexts;

/// <summary>
/// Контекст базы данных для работы с чатами.
/// </summary>
/// <param name="options">Опции контекста.</param>
public class ConversationDbContext(DbContextOptions<ConversationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция чатов.
    /// </summary>
    public DbSet<Chat> Chats => Set<Chat>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.ChatConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.MessageConfiguration());

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
