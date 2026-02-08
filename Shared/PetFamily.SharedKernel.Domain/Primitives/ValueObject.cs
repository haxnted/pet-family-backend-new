namespace PetFamily.SharedKernel.Domain.Primitives;

/// <summary>
/// Базовый класс для всех объектов-значений (Value Object),
/// реализующий сравнение и операторы равенства.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Возвращает компоненты, участвующие в сравнении объектов-значений.
    /// Каждый наследник обязан их определить.
    /// </summary>
    /// <returns>Перечисление компонентов, участвующих в равенстве.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(0, (hash, component) =>
                HashCode.Combine(hash, component?.GetHashCode() ?? 0));
    }

    /// <summary>
    /// Универсальный оператор равенства для объектов-значений.
    /// </summary>
    /// <param name="left">Первый объект.</param>
    /// <param name="right">Второй объект.</param>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Универсальный оператор неравенства для объектов-значений.
    /// </summary>
    /// <param name="left">Первый объект.</param>
    /// <param name="right">Второй объект.</param>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
}