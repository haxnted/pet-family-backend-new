using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Conversation.Domain.Aggregates.ValueObjects.Properties;

/// <summary>
/// Объект-значение Текст сообщения.
/// </summary>
public sealed class MessageText : ValueObject, IComparable<MessageText>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Максимально допустимая длина текста сообщения.
    /// </summary>
    public const int MaxLength = 2000;

    private MessageText(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания текста сообщения <see cref="MessageText"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    /// <exception cref="DomainException">
    /// Если текст пустой или превышает <see cref="MaxLength"/>.
    /// </exception>
    public static MessageText Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length > MaxLength)
        {
            throw new DomainException($"Текст сообщения не должен превышать {MaxLength} символов.");
        }

        return new MessageText(value.Trim());
    }

    /// <inheritdoc />
    public int CompareTo(MessageText? other)
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