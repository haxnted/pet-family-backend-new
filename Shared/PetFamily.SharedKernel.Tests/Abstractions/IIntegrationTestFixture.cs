namespace PetFamily.SharedKernel.Tests.Abstractions;

/// <summary>
/// Интерфейс для fixture интеграционных тестов.
/// </summary>
public interface IIntegrationTestFixture
{
    /// <summary>
    /// Строка подключения к тестовой БД.
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Указывает, инициализирована ли fixture.
    /// </summary>
    bool IsInitialized { get; }
}