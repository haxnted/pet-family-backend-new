using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace Account.Domain.Aggregates.ValueObjects;

/// <summary>
/// Объект-значение Номер телефона.
/// </summary>
public sealed class PhoneNumber : ValueObject, IComparable<PhoneNumber>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Регулярное выражение для проверки валидности телефона.
    /// </summary>
    private const string PhoneRegex = @"^7\d{10}$";

    /// <summary>
    /// Максимальная длина строки.
    /// </summary>
    public const int MaxLength = 11;

    private PhoneNumber(string value) => Value = value;

    /// <summary>
    /// Фабричный метод для создания номера телефона <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="value">Строковое значение номера телефона в формате 7XXXXXXXXXX.</param>
    /// <exception cref="DomainException">
    /// Если номер некорректный.
    /// </exception>
    public static PhoneNumber Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        value = value.Trim();

        if (!Regex.IsMatch(value, PhoneRegex))
            throw new DomainException("Номер телефона должен быть в формате 7XXXXXXXXXX.");

        return new PhoneNumber(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public int CompareTo(PhoneNumber? other)
    {
        if (other == null) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
