using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение Адрес.
/// </summary>
public sealed class Address : ValueObject, IComparable<Address>
{
    /// <summary>
    /// Улица.
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// Город.
    /// </summary>
    public string City { get; }

    /// <summary>
    /// Область или штат.
    /// </summary>
    public string State { get; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public string ZipCode { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    /// <summary>
    /// Фабричный метод для создания объекта <see cref="Address"/>.
    /// </summary>
    /// <param name="street">Улица.</param>
    /// <param name="city">Город.</param>
    /// <param name="state">Область или штат.</param>
    /// <param name="zipCode">Почтовый индекс.</param>
    /// <exception cref="DomainException">
    /// Если одно из значений некорректно.
    /// </exception>
    public static Address Of(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Поле 'Улица' не может быть пустым.");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("Поле 'Город' не может быть пустым.");

        if (string.IsNullOrWhiteSpace(state))
            throw new DomainException("Поле 'Регион/Штат' не может быть пустым.");

        if (string.IsNullOrWhiteSpace(zipCode))
            throw new DomainException("Поле 'Почтовый индекс' не может быть пустым.");

        return new Address(street.Trim(), city.Trim(), state.Trim(), zipCode.Trim());
    }

    /// <inheritdoc/>
    public int CompareTo(Address? other)
    {
        if (other == null) return 1;
        var cityComparison = string.Compare(City, other.City, StringComparison.Ordinal);
        if (cityComparison != 0) return cityComparison;
        var streetComparison = string.Compare(Street, other.Street, StringComparison.Ordinal);
        return streetComparison != 0
            ? streetComparison
            : string.Compare(ZipCode, other.ZipCode, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}