using System;
using System.Threading.Tasks;
using PetFamily.SharedKernel.Tests.Abstractions;
using Testcontainers.PostgreSql;
using Xunit;

namespace PetFamily.SharedKernel.Tests.Fixtures;

/// <summary>
/// Fixture для PostgreSQL контейнера.
/// </summary>
public sealed class PostgresContainerFixture : IAsyncLifetime, IIntegrationTestFixture
{
    private PostgreSqlContainer? _container;

    public string ConnectionString => _container?.GetConnectionString()
        ?? throw new InvalidOperationException("Container not initialized");

    public bool IsInitialized => _container != null;

    public string Host => _container?.Hostname
        ?? throw new InvalidOperationException("Container not initialized");

    public int Port => _container?.GetMappedPublicPort(5432)
        ?? throw new InvalidOperationException("Container not initialized");

    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17.2-alpine")
            .WithDatabase("test_db")
            .WithUsername("test_user")
            .WithPassword("test_password")
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