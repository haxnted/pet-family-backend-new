using System;
using System.Threading.Tasks;
using PetFamily.SharedKernel.Tests.Abstractions;
using Testcontainers.RabbitMq;
using Xunit;

namespace PetFamily.SharedKernel.Tests.Fixtures;

/// <summary>
/// Fixture для RabbitMQ контейнера с использованием Testcontainers.
/// Автоматически запускает и останавливает RabbitMQ в Docker контейнере для интеграционных тестов.
/// </summary>
public sealed class RabbitMqContainerFixture : IAsyncLifetime, IIntegrationTestFixture
{
    private RabbitMqContainer? _container;

    /// <summary>
    /// Получает строку подключения к RabbitMQ.
    /// </summary>
    /// <exception cref="InvalidOperationException">Контейнер не инициализирован.</exception>
    public string ConnectionString => _container?.GetConnectionString()
        ?? throw new InvalidOperationException("Container not initialized");

    /// <summary>
    /// Проверяет, инициализирован ли контейнер.
    /// </summary>
    public bool IsInitialized => _container != null;

    /// <summary>
    /// Получает имя хоста контейнера.
    /// </summary>
    /// <exception cref="InvalidOperationException">Контейнер не инициализирован.</exception>
    public string Host => _container?.Hostname
        ?? throw new InvalidOperationException("Container not initialized");

    /// <summary>
    /// Получает публичный порт для AMQP подключения (5672).
    /// </summary>
    /// <exception cref="InvalidOperationException">Контейнер не инициализирован.</exception>
    public int Port => _container?.GetMappedPublicPort(5672)
        ?? throw new InvalidOperationException("Container not initialized");

    /// <summary>
    /// Получает публичный порт для веб-интерфейса управления (15672).
    /// </summary>
    /// <exception cref="InvalidOperationException">Контейнер не инициализирован.</exception>
    public int ManagementPort => _container?.GetMappedPublicPort(15672)
        ?? throw new InvalidOperationException("Container not initialized");

    /// <summary>
    /// Инициализирует и запускает RabbitMQ контейнер.
    /// </summary>
    public async Task InitializeAsync()
    {
        _container = new RabbitMqBuilder()
            .WithImage("rabbitmq:4.0-management-alpine")
            .WithUsername("guest")
            .WithPassword("guest")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    /// <summary>
    /// Останавливает и удаляет RabbitMQ контейнер.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (_container != null)
        {
            await _container.DisposeAsync();
        }
    }
}