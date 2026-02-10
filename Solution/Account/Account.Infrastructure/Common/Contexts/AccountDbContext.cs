using MassTransit;
using Microsoft.EntityFrameworkCore;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Infrastructure.Common.Contexts;

/// <summary>
/// Контекст базы данных для работы с аккаунтами.
/// </summary>
/// <param name="options">Опции контекста.</param>
public class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция аккаунтов.
    /// </summary>
    public DbSet<DomainAccount> Accounts => Set<DomainAccount>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.AccountConfiguration());

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
