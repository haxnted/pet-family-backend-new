using PetFamily.SharedKernel.Domain.Exceptions;

namespace Conversation.Domain.Aggregates.ValueObjects.Identifiers;

/// <summary>
/// Объект-значение Идентификатор Чата.
/// </summary>
public readonly struct ChatId : IEquatable<ChatId>, IComparable<ChatId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private ChatId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Чата <see cref="ChatId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static ChatId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор чата не может быть пустым.");
        }

        return new ChatId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ChatId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(ChatId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(ChatId other)
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
    public static bool operator ==(ChatId left, ChatId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    public static bool operator !=(ChatId left, ChatId right)
    {
        return !Equals(left, right);
    }
}