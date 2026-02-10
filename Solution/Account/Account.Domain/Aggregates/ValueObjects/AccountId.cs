using PetFamily.SharedKernel.Domain.Exceptions;

namespace Account.Domain.Aggregates.ValueObjects;

/// <summary>
/// Объект-значение Идентификатор Аккаунта.
/// </summary>
public readonly struct AccountId : IEquatable<AccountId>, IComparable<AccountId>
{
    /// <summary>
    /// Значение.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода <see cref="Of(Guid)"/>.
    /// </summary>
    /// <param name="value">Значение.</param>
    private AccountId(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания идентификатора Аккаунта <see cref="AccountId"/>.
    /// </summary>
    /// <param name="value">Идентификатор.</param>
    /// <exception cref="DomainException">
    /// Если идентификатор пустой.
    /// </exception>
    public static AccountId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException("Идентификатор аккаунта не может быть пустым.");
        }

        return new AccountId(value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is AccountId other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(AccountId other)
    {
        return other.Value == Value;
    }

    /// <inheritdoc />
    public int CompareTo(AccountId other)
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
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator ==(AccountId left, AccountId right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Оператор отрицания.
    /// </summary>
    /// <param name="left">Первый идентификатор.</param>
    /// <param name="right">Второй идентификатор.</param>
    public static bool operator !=(AccountId left, AccountId right)
    {
        return !Equals(left, right);
    }
}
