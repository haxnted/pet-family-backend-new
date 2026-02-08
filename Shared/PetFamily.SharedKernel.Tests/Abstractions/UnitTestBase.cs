namespace PetFamily.SharedKernel.Tests.Abstractions;

/// <summary>
/// Базовый класс для юнит-тестов.
/// </summary>
public abstract class UnitTestBase
{
    /// <summary>
    /// Создает CancellationToken с указанным таймаутом.
    /// </summary>
    protected static CancellationToken CreateCancellationToken(int timeoutSeconds = 30)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
        return cts.Token;
    }

    /// <summary>
    /// Выполняет действие и ожидает исключение указанного типа.
    /// </summary>
    protected static TException AssertThrows<TException>(Action action)
        where TException : Exception
    {
        return Assert.Throws<TException>(action);
    }

    /// <summary>
    /// Асинхронно выполняет действие и ожидает исключение указанного типа.
    /// </summary>
    protected static async Task<TException> AssertThrowsAsync<TException>(Func<Task> action)
        where TException : Exception
    {
        return await Assert.ThrowsAsync<TException>(action);
    }
}