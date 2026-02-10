using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.Infrastructure.Common.Contexts;

/// <summary>
/// Фабрика для создания контекста базы данных <see cref="AccountDbContext"/> в режиме проектирования.
/// </summary>
public class AccountDbContextFactory
    : IDesignTimeDbContextFactory<AccountDbContext>
{
    /// <summary>
    /// Создаёт экземпляр контекста базы данных <see cref="AccountDbContext"/> в режиме проектирования.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public AccountDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("AccountDbContext")
                               ?? "Host=localhost;Port=5437;Database=account;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new AccountDbContext(options);
    }
}
