namespace PetFamily.SharedKernel.Domain.Errors;

/// <summary>
/// Представляет собой ошибку которая возникла в системе.
/// </summary>
public sealed class Error
{
    /// <summary>
    /// Представляет собой ошибку которая возникла в системе.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="type">Тип ошибки.</param>
    private Error(string message, ErrorType type)
    {
        Message = message;
        Type = type;
    }

    /// <summary>
    /// Создает ошибку валидации
    /// </summary>
    public static Error Validation(string message) => new(message, ErrorType.Validation);

    /// <summary>
    /// Создает ошибку "не найдено"
    /// </summary>
    public static Error NotFound(string message) => new(message, ErrorType.NotFound);

    /// <summary>
    /// Создает ошибку конфликта
    /// </summary>
    public static Error Conflict(string message) => new(message, ErrorType.Conflict);

    /// <summary>
    /// Создает ошибку авторизации
    /// </summary>
    public static Error Unauthorized(string message) => new(message, ErrorType.Unauthorized);

    /// <summary>
    /// Создает ошибку доступа
    /// </summary>
    public static Error Forbidden(string message) => new(message, ErrorType.Forbidden);

    /// <summary>
    /// Создает ошибку внутреннего сервера
    /// </summary>
    public static Error Failure(string message) => new(message, ErrorType.Failure);

    /// <summary>Сообщение об ошибке.</summary>
    public string Message { get; init; }

    /// <summary>Тип ошибки.</summary>
    public ErrorType Type { get; init; }

    public void Deconstruct(out string message, out ErrorType type)
    {
        message = Message;
        type = Type;
    }
}