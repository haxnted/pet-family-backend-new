using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;

/// <summary>
/// Идентификатор Животного.
/// </summary>
public readonly struct PetId : IEquatable<PetId>, IComparable<PetId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    private PetId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Животного <see cref="PetId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static PetId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор животного не может быть пустым.");
        }

        return new PetId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is PetId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(PetId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(PetId other)
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
    /// <exception cref="NotImplementedException"></exception>
    public static bool operator ==(PetId left, PetId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(PetId left, PetId right)
    {
        return !Equals(left, right);
    }
}
