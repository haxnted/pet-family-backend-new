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
    /// <typeparam name="TProgram">Тип программы (точка входа приложения).</typeparam>
    /// <param name="factory">Фабрика веб-приложения.</param>
    /// <param name="baseAddress">Базовый адрес для HTTP клиента.</param>
    /// <returns>Настроенный HTTP клиент.</returns>
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
    /// <typeparam name="TProgram">Тип программы (точка входа приложения).</typeparam>
    /// <param name="factory">Фабрика веб-приложения.</param>
    /// <param name="action">Действие для выполнения с IServiceProvider.</param>
    public static async Task ExecuteInScopeAsync<TProgram>(
        this WebApplicationFactory<TProgram> factory,
        Func<IServiceProvider, Task> action) where TProgram : class
    {
        using var scope = factory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    /// <summary>
    /// Выполняет асинхронное действие с DbContext в области видимости.
    /// </summary>
    /// <typeparam name="TProgram">Тип программы (точка входа приложения).</typeparam>
    /// <typeparam name="TDbContext">Тип контекста базы данных.</typeparam>
    /// <param name="factory">Фабрика веб-приложения.</param>
    /// <param name="action">Действие для выполнения с DbContext.</param>
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