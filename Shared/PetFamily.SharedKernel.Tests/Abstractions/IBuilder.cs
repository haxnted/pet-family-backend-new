namespace PetFamily.SharedKernel.Tests.Abstractions;

/// <summary>
/// Базовый интерфейс для Test Data Builder pattern.
/// </summary>
/// <typeparam name="T">Тип создаваемого объекта.</typeparam>
public interface IBuilder<out T>
{
    /// <summary>
    /// Создает и возвращает сконфигурированный объект.
    /// </summary>
    T Build();
}

/// <summary>
/// Расширенный интерфейс builder с поддержкой асинхронного создания.
/// </summary>
/// <typeparam name="T">Тип создаваемого объекта.</typeparam>
public interface IAsyncBuilder<T> : IBuilder<T>
{
    /// <summary>
    /// Асинхронно создает и возвращает сконфигурированный объект.
    /// </summary>
    Task<T> BuildAsync(CancellationToken cancellationToken = default);
}
