namespace PetFamily.SharedKernel.Application.Exceptions;

/// <summary>
/// Исключение для ошибок бизнес-логики на уровне use case (сервисов).
/// Используется для валидаций и операций, которые не относятся к доменным инвариантам.
/// </summary>
public class UseCaseException : Exception
{
    /// <summary>
    /// Создает новое исключение use case с сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public UseCaseException(string message) : base(message)
    {
    }

    /// <summary>
    /// Создает новое исключение use case с сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public UseCaseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
