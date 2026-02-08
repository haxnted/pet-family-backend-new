namespace PetFamily.SharedKernel.Domain.Primitives;

/// <summary>
/// Абстрактный класс, представляющий сущность.
/// </summary>
/// <typeparam name="TId">Тип идентификатора.</typeparam>
public abstract class Entity<TId> where TId : notnull
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// EF Constructor.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    protected Entity(TId id) => Id = id;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;
        
        var other = (Entity<TId>)obj;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    /// <inheritdoc/>
    public override int GetHashCode() =>
        (GetType().FullName + Id).GetHashCode();
}