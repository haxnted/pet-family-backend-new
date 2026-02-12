using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notification.Infrastructure.Common;

/// <summary>
/// Фабрика для создания контекста базы данных <see cref="NotificationDbContext"/> в режиме проектирования.
/// </summary>
public class NotificationDbContextFactory
    : IDesignTimeDbContextFactory<NotificationDbContext>
{
    /// <summary>
    /// Создаёт экземпляр контекста базы данных <see cref="NotificationDbContext"/> в режиме проектирования.
    /// </summary>
    /// <param name="args">Аргументы.</param>
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("VolunteerManagementDbContext")
                               ??
                               "Host=localhost;Port=5435;Database=notification;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new NotificationDbContext(options);
    }
}