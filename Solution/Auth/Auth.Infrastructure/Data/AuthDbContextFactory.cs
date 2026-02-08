using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth.Infrastructure.Data
{
    /// <summary>
    /// Фабрика для создания контекста базы данных <see cref="AuthDbContext"/> в режиме проектирования.
    /// </summary>
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        /// <summary>
        /// Создаёт экземпляр контекста базы данных <see cref="AuthDbContext"/> в режиме проектирования.
        /// </summary>
        /// <param name="args">Аргументы.</param>
        public AuthDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("AuthDbContext")
                ?? "Host=localhost;Port=5432;Database=auth;Username=postgres;Password=postgres";

            var options = new DbContextOptionsBuilder<AuthDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new AuthDbContext(options);
        }
    }
}
