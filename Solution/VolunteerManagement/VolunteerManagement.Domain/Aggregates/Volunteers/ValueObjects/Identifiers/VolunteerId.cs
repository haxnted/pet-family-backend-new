using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;

/// <summary>
/// Объект-значение Идентификатор Волонтёра.
/// </summary>
public readonly struct VolunteerId : IEquatable<VolunteerId>, IComparable<VolunteerId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Волонтёра <see cref="VolunteerId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static VolunteerId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор волонтёра не может быть пустым.");
        }

        return new VolunteerId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is VolunteerId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(VolunteerId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(VolunteerId other)
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
    public static bool operator ==(VolunteerId left, VolunteerId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(VolunteerId left, VolunteerId right)
    {
        return !Equals(left, right);
    }
}