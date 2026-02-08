using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение позиция животного в коллекции животных.
/// </summary>
public sealed class Position : ValueObject, IComparable<Position>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода. 
    /// </summary>
    /// <param name="value">Позиция.</param>
    private Position(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания позиции <see cref="Position"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <exception cref="DomainException">
    /// Если позиция меньше либо равна нулю.
    /// </exception>
    public static Position Of(int value)
    {
        if (value < 0)
        {
            throw new DomainException("Позиция животного не должна быть меньше нуля");
        }

        return new Position(value);
    }

    /// <inheritdoc/>
    public int CompareTo(Position? other)
    {
        if (other == null) return 1;
        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}