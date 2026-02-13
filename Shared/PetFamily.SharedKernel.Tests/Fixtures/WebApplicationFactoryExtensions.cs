using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.SharedKernel.Tests.Fixtures;

/// <summary>
/// Расширения для WebApplicationFactory для упрощения работы в интеграционных тестах.
/// </summary>
public static class WebApplicationFactoryExtensions
{
    /// <summary>
    /// Создаёт HTTP клиент с указанным базовым адресом.
    /// </summary>
    public static HttpClient CreateClientWithBaseAddress<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        string baseAddress = "https://localhost") where TProgram : class
    {
        var client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        client.BaseAddress = new Uri(baseAddress);
        return client;
    }

    /// <summary>
    /// Выполняет асинхронное действие в области видимости сервисов.
    /// </summary>
    public static async Task ExecuteInScopeAsync<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        Func<IServiceProvider, Task> action) where TProgram : class
    {
        using var scope = factory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    /// <summary>
    /// Выполняет асинхронное действие с DbContext.
    /// </summary>
    public static async Task ExecuteWithDbContextAsync<TProgram, TDbContext>(
        this WebApplicationFactory<TProgram> factory,
        Func<TDbContext, Task> action)
        where TProgram : class
        where TDbContext : DbContext
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await action(dbContext);
    }
}