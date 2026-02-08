using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

/// <summary>
/// Объект-значение Вид Животного.
/// </summary>
public sealed class AnimalKind : ValueObject, IComparable<AnimalKind>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }
    
    /// <summary>
    /// Минимально допустимая длина.
    /// </summary>
    public const int MinLength = 3;

    /// <summary>
    /// Максимально допустимая длина.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Регулярное выражение для недопустимости спец символов в строке.
    /// </summary>
    private const string NamePattern = @"^[A-Za-zА-Яа-яЁё-]+$";
    
    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private AnimalKind(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания вида животного <see cref="AnimalKind"/>.
    /// </summary>
    /// <param name="value">Значение</param>
    /// <exception cref="DomainException">
    /// Если описание меньше <see cref="MinLength"/>, либо превышает <see cref="MaxLength"/>.
    /// </exception>
    public static AnimalKind Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length is < MinLength or > MaxLength)
        {
            throw new DomainException(
                $"Никнейм не должен превышать {MaxLength} и не быть меньше {MinLength} символов.");
        }

        if (!Regex.IsMatch(value, NamePattern))
        {
            throw new DomainException($"Вид животного не может содержать спец символы.");
        }

        return new AnimalKind(value.Trim());
    }

    /// <inheritdoc/>
    public int CompareTo(AnimalKind? other)
    {
        if (other == null) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
