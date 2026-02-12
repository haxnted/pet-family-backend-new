using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Domain.Aggregates.ValueObjects.Properties;

/// <summary>
/// Объект-значение Опыт (в годах).
/// </summary>
public sealed class AgeExperience : ValueObject, IComparable<AgeExperience>
{
    /// <summary>
    /// Количество лет опыта.
    /// </summary>
    public int Value { get; }

    private AgeExperience(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания объекта <see cref="AgeExperience"/>.
    /// </summary>
    /// <param name="value">Количество лет опыта.</param>
    /// <exception cref="DomainException">
    /// Если значение меньше нуля.
    /// </exception>
    public static AgeExperience Of(int value)
    {
        if (value < 0)
            throw new DomainException("Опыт не может быть отрицательным.");

        return new AgeExperience(value);
    }

    /// <inheritdoc/>
    public int CompareTo(AgeExperience? other)
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
