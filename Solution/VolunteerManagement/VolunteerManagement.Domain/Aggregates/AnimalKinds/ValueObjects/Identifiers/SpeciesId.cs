using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;

/// <summary>
/// Идентификатор Породы.
/// </summary>
public readonly struct SpeciesId : IEquatable<SpeciesId>, IComparable<SpeciesId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    private SpeciesId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Породы <see cref="SpeciesId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static SpeciesId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор породы не может быть пустым.");
        }

        return new SpeciesId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is SpeciesId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(SpeciesId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(SpeciesId other)
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
    public static bool operator ==(SpeciesId left, SpeciesId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(SpeciesId left, SpeciesId right)
    {
        return !Equals(left, right);
    }
}