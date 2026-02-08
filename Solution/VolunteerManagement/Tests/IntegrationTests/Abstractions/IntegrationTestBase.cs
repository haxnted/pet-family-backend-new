using Microsoft.Extensions.DependencyInjection;
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
        _respawner = await Respawner.CreateAsync(Factory.ConnectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await _respawner.ResetAsync(Factory.ConnectionString);
    }

    public virtual async Task DisposeAsync()
    {
        if (_respawner != null)
        {
            await _respawner.ResetAsync(Factory.ConnectionString);
        }
    }
}
