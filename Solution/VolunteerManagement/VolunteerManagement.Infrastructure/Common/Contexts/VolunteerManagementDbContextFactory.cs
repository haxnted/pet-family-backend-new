using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VolunteerManagement.Infrastructure.Common.Contexts;

/// <summary>
/// Фабрика для создания контекста базы данных <see cref="VolunteerManagementDbContext"/> в режиме проектирования.
/// </summary>
public class VolunteerManagementDbContextFactory
    : IDesignTimeDbContextFactory<VolunteerManagementDbContext>
{
    /// <summary>
    /// Создаёт экземпляр контекста базы данных <see cref="VolunteerManagementDbContext"/> в режиме проектирования.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public VolunteerManagementDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("VolunteerManagementDbContext")
                               ?? "Host=localhost;Port=5433;Database=volunteer_management;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<VolunteerManagementDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new VolunteerManagementDbContext(options);
    }
}