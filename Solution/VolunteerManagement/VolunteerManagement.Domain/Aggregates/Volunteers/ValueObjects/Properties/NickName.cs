using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение Никнейм.
/// </summary>
public sealed class NickName : ValueObject, IComparable<NickName>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Минимально допустимая длина.
    /// </summary>
    public const int MinLength = 10;

    /// <summary>
    /// Максимально допустимая длина.
    /// </summary>
    public const int MaxLength = 500;

    /// <summary>
    /// Регулярное выражение для недопустимости спец символов в строке.
    /// </summary>
    private const string NamePattern = @"^[A-Za-zА-Яа-яЁё-]+$";

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private NickName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания никнейма <see cref="NickName"/>.
    /// </summary>
    /// <param name="name">Значение.</param>
    /// <exception cref="DomainException">
    /// Если описание меньше <see cref="MinLength"/>, либо превышает <see cref="MaxLength"/>.
    /// </exception>
    public static NickName Of(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (!Regex.IsMatch(name, NamePattern))
        {
            throw new DomainException($"{name} содержит недопустимые символы. Разрешены только буквы и дефис.");
        }

        if (name.Length is < MinLength or > MaxLength)
        {
            throw new DomainException(
                $"Никнейм не должен превышать {MaxLength} и не быть меньше {MinLength} символов.");
        }


        return new NickName(name.Trim());
    }

    /// <inheritdoc/>
    public int CompareTo(NickName? other)
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