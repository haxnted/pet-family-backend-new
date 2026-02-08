namespace PetFamily.SharedKernel.Application.Exceptions;

/// <summary>
/// Исключение, выбрасываемое когда сущность не найдена в репозитории.
/// Всегда маппится на 404 Not Found.
/// </summary>
/// <typeparam name="TEntity">Тип сущности, которая не была найдена.</typeparam>
public class EntityNotFoundException<TEntity> : Exception
{
    /// <summary>
    /// Идентификатор сущности, которая не была найдена.
    /// </summary>
    public object EntityId { get; }

    /// <summary>
    /// Имя типа сущности.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Создает новое исключение EntityNotFoundException.
    /// </summary>
    /// <param name="entityId">Идентификатор сущности.</param>
    public EntityNotFoundException(object entityId)
        : base($"{typeof(TEntity).Name} с идентификатором '{entityId}' не найден.")
    {
        EntityId = entityId;
        EntityName = typeof(TEntity).Name;
    }

    /// <summary>
    /// Создает новое исключение EntityNotFoundException с кастомным сообщением.
    /// </summary>
    /// <param name="entityId">Идентификатор сущности.</param>
    /// <param name="message">Пользовательское сообщение об ошибке.</param>
    public EntityNotFoundException(object entityId, string message) : base(message)
    {
        EntityId = entityId;
        EntityName = typeof(TEntity).Name;
    }

    /// <summary>
    /// Создает новое исключение EntityNotFoundException с внутренним исключением.
    /// </summary>
    /// <param name="entityId">Идентификатор сущности.</param>
    /// <param name="innerException">Внутреннее исключение.</param>
    public EntityNotFoundException(object entityId, Exception innerException)
        : base($"{typeof(TEntity).Name} с идентификатором '{entityId}' не найден.", innerException)
    {
        EntityId = entityId;
        EntityName = typeof(TEntity).Name;
    }
}
