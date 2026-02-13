using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PetFamily.SharedKernel.Tests.Fixtures;

/// <summary>
/// Базовая фабрика для интеграционных тестов API.
/// </summary>
public class CustomWebApplicationFactory<TProgram, TDbContext>(
    string connectionString,
    Action<IServiceCollection>? configureServices = null)
    : WebApplicationFactory<TProgram>
    where TProgram : class
    where TDbContext : DbContext
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<TDbContext>>();
            services.RemoveAll<TDbContext>();

            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            configureServices?.Invoke(services);

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }
}
