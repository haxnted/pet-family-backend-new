namespace PetFamily.SharedKernel.Domain.Primitives;

/// <summary>
/// Абстрактный класс, который добавляет мягкое удаление для сущности.
/// </summary>
/// <typeparam name="TId">Тип идентификатора.</typeparam>
public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    protected SoftDeletableEntity(TId id) : base(id) { }

    /// <summary>
    /// Флаг мягкого удаления.
    /// </summary>
    public bool IsDeleted { get; private set; }
    
    /// <summary>
    /// Время мягкого удаления.
    /// </summary>
    public DateTime? DeletedAt { get; private set; }
    
    /// <summary>
    /// Применить мягкое удаление к сущности.
    /// </summary>
    public virtual void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Восстановить сущность.
    /// </summary>
    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
    
}
