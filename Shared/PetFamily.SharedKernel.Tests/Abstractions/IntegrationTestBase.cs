using Respawn;

namespace PetFamily.SharedKernel.Tests.Abstractions;

/// <summary>
/// Базовый класс для интеграционных тестов с поддержкой очистки БД.
/// </summary>
/// <typeparam name="TFixture">Тип fixture для интеграционных тестов.</typeparam>
public abstract class IntegrationTestBase<TFixture> : IAsyncLifetime
    where TFixture : class, IIntegrationTestFixture
{
    protected readonly TFixture Fixture;
    private Respawner? _respawner;

    protected IntegrationTestBase(TFixture fixture)
    {
        Fixture = fixture;
    }

    /// <summary>
    /// Получает строку подключения к БД.
    /// </summary>
    protected abstract string GetConnectionString();

    public virtual async Task InitializeAsync()
    {
        var connectionString = GetConnectionString();

        _respawner = await Respawner.CreateAsync(connectionString, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public virtual async Task DisposeAsync()
    {
        if (_respawner != null)
        {
            await _respawner.ResetAsync(GetConnectionString());
        }
    }

    /// <summary>
    /// Сбрасывает состояние БД между тестами.
    /// </summary>
    protected async Task ResetDatabaseAsync()
    {
        if (_respawner != null)
        {
            await _respawner.ResetAsync(GetConnectionString());
        }
    }
}
