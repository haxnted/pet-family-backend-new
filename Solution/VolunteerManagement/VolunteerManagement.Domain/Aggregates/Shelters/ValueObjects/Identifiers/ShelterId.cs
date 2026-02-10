using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

/// <summary>
/// Объект-значение Идентификатор Приюта.
/// </summary>
public readonly struct ShelterId : IEquatable<ShelterId>, IComparable<ShelterId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private ShelterId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Приюта <see cref="ShelterId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static ShelterId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор приюта не может быть пустым.");
        }

        return new ShelterId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ShelterId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(ShelterId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(ShelterId other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Получить Hash.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Оператор сравнения.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator ==(ShelterId left, ShelterId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(ShelterId left, ShelterId right)
    {
        return !Equals(left, right);
    }
}
