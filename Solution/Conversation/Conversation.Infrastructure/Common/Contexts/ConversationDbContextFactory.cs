using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Conversation.Infrastructure.Common.Contexts;

/// <summary>
/// Фабрика для создания контекста базы данных <see cref="ConversationDbContext"/> в режиме проектирования.
/// </summary>
public class ConversationDbContextFactory
    : IDesignTimeDbContextFactory<ConversationDbContext>
{
    /// <summary>
    /// Создаёт экземпляр контекста базы данных <see cref="ConversationDbContext"/> в режиме проектирования.
    /// </summary>
    public ConversationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConversationDbContext")
                               ?? "Host=localhost;Port=5438;Database=conversation;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<ConversationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new ConversationDbContext(options);
    }
}
