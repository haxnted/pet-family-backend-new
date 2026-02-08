namespace PetFamily.SharedKernel.Domain.Errors;

/// <summary>
/// Типы ошибок для классификации
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Ошибка валидации
    /// </summary>
    Validation,

    /// <summary>
    /// Сущность не найдена
    /// </summary>
    NotFound,

    /// <summary>
    /// Конфликт (например, дубликат)
    /// </summary>
    Conflict,

    /// <summary>
    /// Ошибка авторизации
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Недостаточно прав
    /// </summary>
    Forbidden,

    /// <summary>
    /// Внутренняя ошибка сервера
    /// </summary>
    Failure
}
