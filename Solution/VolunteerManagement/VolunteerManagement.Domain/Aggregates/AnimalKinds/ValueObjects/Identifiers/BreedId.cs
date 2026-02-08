using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;

/// <summary>
/// Идентификатор Порода животного.
/// </summary>
public readonly struct BreedId : IEquatable<BreedId>, IComparable<BreedId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    private BreedId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Породы животного <see cref="BreedId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static BreedId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор животного не может быть пустым.");
        }

        return new BreedId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is BreedId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(BreedId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(BreedId other)
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
    public static bool operator ==(BreedId left, BreedId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(BreedId left, BreedId right)
    {
        return !Equals(left, right);
    }
}
