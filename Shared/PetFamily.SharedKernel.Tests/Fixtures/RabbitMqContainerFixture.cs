using PetFamily.SharedKernel.Tests.Abstractions;
using Testcontainers.RabbitMq;

namespace PetFamily.SharedKernel.Tests.Fixtures;

/// <summary>
/// Fixture для RabbitMQ контейнера с использованием Testcontainers.
/// </summary>
public sealed class RabbitMqContainerFixture : IAsyncLifetime, IIntegrationTestFixture
{
    private RabbitMqContainer? _container;

    public string ConnectionString => _container?.GetConnectionString()
        ?? throw new InvalidOperationException("Container not initialized");

    public bool IsInitialized => _container != null;

    public string Host => _container?.Hostname
        ?? throw new InvalidOperationException("Container not initialized");

    public int Port => _container?.GetMappedPublicPort(5672)
        ?? throw new InvalidOperationException("Container not initialized");

    public int ManagementPort => _container?.GetMappedPublicPort(15672)
        ?? throw new InvalidOperationException("Container not initialized");

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

    public async Task DisposeAsync()
    {
        if (_container != null)
        {
            await _container.DisposeAsync();
        }
    }
}
