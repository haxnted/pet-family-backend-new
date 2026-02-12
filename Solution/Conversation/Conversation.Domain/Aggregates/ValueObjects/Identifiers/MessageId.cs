using PetFamily.SharedKernel.Domain.Exceptions;

namespace Conversation.Domain.Aggregates.ValueObjects.Identifiers;

/// <summary>
/// Объект-значение Идентификатор Сообщения.
/// </summary>
public readonly struct MessageId : IEquatable<MessageId>, IComparable<MessageId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private MessageId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Сообщения <see cref="MessageId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static MessageId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор сообщения не может быть пустым.");
        }

        return new MessageId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is MessageId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(MessageId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(MessageId other)
    {
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Получить Hash.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Оператор сравнения.
    /// </summary>
    public static bool operator ==(MessageId left, MessageId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    public static bool operator !=(MessageId left, MessageId right)
    {
        return !Equals(left, right);
    }
}