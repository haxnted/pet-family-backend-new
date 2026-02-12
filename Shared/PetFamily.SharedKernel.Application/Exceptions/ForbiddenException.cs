namespace PetFamily.SharedKernel.Application.Exceptions;

/// <summary>
/// Исключение, указывающее на отсутствие прав доступа (403 Forbidden).
/// </summary>
public class ForbiddenException : Exception
{
    /// <summary>
    /// Создает экземпляр ForbiddenException с указанным сообщением.
    /// </summary>
    public ForbiddenException(string message) : base(message) { }
}
