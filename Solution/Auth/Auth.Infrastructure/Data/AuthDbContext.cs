using Auth.Core.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

/// <summary>
/// Контекст базы данных для Auth модуля.
/// </summary>
public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Пользователи.
    /// </summary>
    public DbSet<User> Users => Set<User>();
    
    /// <summary>
    /// Refresh токены.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);

        // MassTransit Outbox/Inbox tables
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}