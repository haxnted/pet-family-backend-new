using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetFamily.SharedKernel.Tests.Abstractions;
using Testcontainers.PostgreSql;
using VolunteerManagement.Infrastructure.Common.Contexts;

namespace VolunteerManagement.Tests.Integration.Fixtures;

public sealed class VolunteerManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime, IIntegrationTestFixture
{
    private PostgreSqlContainer? _dbContainer;

    public string ConnectionString => _dbContainer?.GetConnectionString()
        ?? throw new InvalidOperationException("Database container not initialized");

    public bool IsInitialized => _dbContainer != null;

    public async Task InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.2-alpine")
            .WithDatabase("volunteer_management_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .WithCleanUp(true)
            .Build();

        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        if (_dbContainer != null)
        {
            await _dbContainer.DisposeAsync();
        }

        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<VolunteerManagementDbContext>>();
            services.RemoveAll<VolunteerManagementDbContext>();

            services.AddDbContext<VolunteerManagementDbContext>(options =>
            {
                options.UseNpgsql(ConnectionString);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }

    public IServiceScope CreateScope() => Services.CreateScope();

    public VolunteerManagementDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
    }
}

[CollectionDefinition(Name)]
public sealed class VolunteerManagementIntegrationTestCollection
    : ICollectionFixture<VolunteerManagementWebApplicationFactory>
{
    public const string Name = "VolunteerManagementIntegration";
}
