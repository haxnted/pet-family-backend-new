using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

/// <summary>
/// Объект-значение Идентификатор назначения волонтёра.
/// </summary>
public readonly struct VolunteerAssignmentId : IEquatable<VolunteerAssignmentId>, IComparable<VolunteerAssignmentId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private VolunteerAssignmentId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора назначения <see cref="VolunteerAssignmentId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static VolunteerAssignmentId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор назначения волонтёра не может быть пустым.");
        }

        return new VolunteerAssignmentId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is VolunteerAssignmentId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(VolunteerAssignmentId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(VolunteerAssignmentId other)
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
    public static bool operator ==(VolunteerAssignmentId left, VolunteerAssignmentId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(VolunteerAssignmentId left, VolunteerAssignmentId right)
    {
        return !Equals(left, right);
    }
}
