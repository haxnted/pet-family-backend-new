using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Domain.Aggregates.ValueObjects.Properties;

/// <summary>
/// Объект-значение Заголовок чата.
/// </summary>
public sealed class Title : ValueObject, IComparable<Title>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Максимально допустимая длина заголовка.
    /// </summary>
    public const int MaxLength = 100;

    private Title(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания заголовка <see cref="Title"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <exception cref="DomainException">
    /// Если заголовок пустой или превышает <see cref="MaxLength"/>.
    /// </exception>
    public static Title Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length > MaxLength)
        {
            throw new DomainException($"Заголовок чата не должен превышать {MaxLength} символов.");
        }

        return new Title(value.Trim());
    }

    /// <inheritdoc />
    public int CompareTo(Title? other)
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