using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PetFamily.SharedKernel.Infrastructure.Caching;
using PetFamily.SharedKernel.Tests.Abstractions;
using Testcontainers.PostgreSql;
using VolunteerManagement.Infrastructure.Common.Contexts;
using MassTransit;

namespace VolunteerManagement.Tests.Integration.Fixtures;

public sealed class VolunteerManagementWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime, IIntegrationTestFixture
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

            var massTransitDescriptors = services
                .Where(d =>
                    (d.ServiceType.FullName?.Contains("MassTransit") ?? false) ||
                    (d.ImplementationType?.FullName?.Contains("MassTransit") ?? false))
                .ToList();

            foreach (var descriptor in massTransitDescriptors)
                services.Remove(descriptor);

            services.Configure<HealthCheckServiceOptions>(options =>
            {
                var mtChecks = options.Registrations
                    .Where(r => r.Name.StartsWith("masstransit", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var check in mtChecks)
                    options.Registrations.Remove(check);
            });

            services.AddMassTransitTestHarness();

            services.RemoveAll<ICacheService>();
            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddMemoryCache();
        });
    }


    public VolunteerManagementDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<VolunteerManagementDbContext>();
    }
}
