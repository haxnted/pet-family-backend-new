using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;

/// <summary>
/// Объект-значение Название приюта.
/// </summary>
public sealed class ShelterName : ValueObject, IComparable<ShelterName>
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
    public const int MaxLength = 100;

    /// <summary>
    /// Регулярное выражение для допустимых символов.
    /// </summary>
    private const string NamePattern = @"^[A-Za-zА-Яа-яЁё0-9\s\-""]+$";

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private ShelterName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания названия приюта <see cref="ShelterName"/>.
    /// </summary>
    /// <param name="name">Значение.</param>
    /// <exception cref="DomainException">
    /// Если название меньше <see cref="MinLength"/>, либо превышает <see cref="MaxLength"/>.
    /// </exception>
    public static ShelterName Of(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (!Regex.IsMatch(name, NamePattern))
        {
            throw new DomainException(
                $"{name} содержит недопустимые символы. Разрешены буквы, цифры, пробелы и дефис.");
        }

        if (name.Length is < MinLength or > MaxLength)
        {
            throw new DomainException(
                $"Название приюта не должно превышать {MaxLength} и не быть меньше {MinLength} символов.");
        }

        return new ShelterName(name.Trim());
    }

    /// <inheritdoc/>
    public int CompareTo(ShelterName? other)
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
