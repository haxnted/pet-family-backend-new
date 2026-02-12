using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Domain.Aggregates.ValueObjects.Properties;

/// <summary>
/// Объект-значение Описание.
/// </summary>
public sealed class Description : ValueObject, IComparable<Description>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Минимально допустимая длина описания.
    /// </summary>
    public const int MinLength = 10;

    /// <summary>
    /// Максимально допустимая длина описания.
    /// </summary>
    public const int MaxLength = 500;

    private Description(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания описания <see cref="Description"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <exception cref="DomainException">
    /// Если описание меньше <see cref="MinLength"/>, либо превышает <see cref="MaxLength"/>.
    /// </exception>
    public static Description Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            throw new DomainException(
                $"Описание не должно превышать {MaxLength} и не быть меньше {MinLength} символов.");
        }

        return new Description(value.Trim());
    }

    /// <inheritdoc />
    public int CompareTo(Description? other)
    {
        if (other == null) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
