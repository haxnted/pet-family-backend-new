using System;
using System.Net.Http;
using System.Threading.Tasks;
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
public class CustomWebApplicationFactory<TProgram, TDbContext> : WebApplicationFactory<TProgram>
    where TProgram : class
    where TDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly Action<IServiceCollection>? _configureServices;

    /// <summary>
    /// Инициализирует новый экземпляр фабрики для интеграционных тестов.
    /// </summary>
    /// <param name="connectionString">Строка подключения к тестовой базе данных.</param>
    /// <param name="configureServices">Дополнительная настройка сервисов (опционально).</param>
    public CustomWebApplicationFactory(
        string connectionString,
        Action<IServiceCollection>? configureServices = null)
    {
        _connectionString = connectionString;
        _configureServices = configureServices;
    }

    /// <summary>
    /// Настраивает веб-хост для тестирования.
    /// Заменяет продуктовый DbContext на тестовый с PostgreSQL.
    /// </summary>
    /// <param name="builder">Строитель веб-хоста.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<TDbContext>>();
            services.RemoveAll<TDbContext>();

            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(_connectionString);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            _configureServices?.Invoke(services);

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }
}