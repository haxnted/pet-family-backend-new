namespace PetFamily.SharedKernel.Domain.Exceptions;

/// <summary>
/// Исключение для нарушения бизнес-правил в доменном слое.
/// Все доменные исключения - это ошибки валидации (400 Bad Request).
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Создает новое доменное исключение с сообщением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке валидации.</param>
    public DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Создает новое доменное исключение с сообщением и внутренним исключением.
    /// </summary>
    /// <param name="message">Сообщение об ошибке валидации.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}