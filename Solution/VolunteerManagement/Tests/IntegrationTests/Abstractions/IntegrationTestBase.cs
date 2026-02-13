using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using VolunteerManagement.Infrastructure.Common.Contexts;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Abstractions;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public abstract class VolunteerManagementIntegrationTestBase : IAsyncLifetime
{
    protected readonly VolunteerManagementWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    private Respawner? _respawner;
    private NpgsqlConnection? _dbConnection;

    protected VolunteerManagementIntegrationTestBase(VolunteerManagementWebApplicationFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected VolunteerManagementDbContext GetDbContext()
    {
        var scope = Factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
    }

    protected T GetService<T>() where T : notnull
    {
        var scope = Factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    protected async Task ExecuteInScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = Factory.Services.CreateScope();
        await action(scope.ServiceProvider);
    }

    protected async Task ExecuteWithDbContextAsync(Func<VolunteerManagementDbContext, Task> action)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
        await action(dbContext);
        await dbContext.SaveChangesAsync();
    }

    protected async Task<T> InsertAsync<T>(T entity) where T : class
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
        dbContext.Set<T>().Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    protected async Task<T?> FindAsync<T>(params object[] keyValues) where T : class
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
        return await dbContext.Set<T>().FindAsync(keyValues);
    }

    public virtual async Task InitializeAsync()
    {
        _dbConnection = new NpgsqlConnection(Factory.ConnectionString);
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await _respawner.ResetAsync(_dbConnection);
    }

    public virtual async Task DisposeAsync()
    {
        if (_respawner != null && _dbConnection != null)
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        if (_dbConnection != null)
        {
            await _dbConnection.DisposeAsync();
        }
    }
}
