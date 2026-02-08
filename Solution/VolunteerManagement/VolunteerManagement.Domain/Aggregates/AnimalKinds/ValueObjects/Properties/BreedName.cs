using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

/// <summary>
/// Объект-значение Название породы.
/// </summary>
public sealed class BreedName : ValueObject, IComparable<BreedName>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Минимально допустимая длина названия.
    /// </summary>
    public const int MinLength = 3;

    /// <summary>
    /// Максимально допустимая длина названия.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Регулярное выражение для недопустимости спец символов в строке.
    /// </summary>
    private const string NamePattern = @"^[A-Za-zА-Яа-яЁё-]+$";

    /// <summary>
    /// Фабричный метод для создания описания <see cref="BreedName"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private BreedName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания описания <see cref="BreedName"/>
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <exception cref="DomainException">
    /// Если описание меньше <see cref="MinLength"/>, либо превышает <see cref="MaxLength"/>.
    /// </exception>
    public static BreedName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            throw new DomainException(
                $"Описание не должно превышать {MaxLength} и не быть меньше {MinLength} символов.");
        }

        if (!Regex.IsMatch(value, NamePattern))
        {
            throw new DomainException($"Вид животного не может содержать спец символы.");
        }

        return new BreedName(value.Trim());
    }

    /// <inheritdoc />
    public int CompareTo(BreedName? other)
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