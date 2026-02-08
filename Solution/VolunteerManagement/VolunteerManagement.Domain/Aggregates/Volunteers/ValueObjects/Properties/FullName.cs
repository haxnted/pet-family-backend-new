using System.Text.RegularExpressions;
using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение ФИО.
/// </summary>
public sealed class FullName : ValueObject, IComparable<FullName>
{
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string Surname { get; }

    /// <summary>
    /// Отчество.
    /// </summary>
    public string? Patronymic { get; }

    /// <summary>
    /// Максимальная длина строки.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Регулярное выражение для недопустимости спец символов в строке.
    /// </summary>
    private const string NamePattern = @"^[A-Za-zА-Яа-яЁё-]+$";

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="name">Имя.</param>
    /// <param name="surname">Фамилия.</param>
    /// <param name="patronymic">Отчество.</param>
    private FullName(string name, string surname, string? patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    /// <summary>
    /// Фабричный метод для создания ФИО <see cref="FullName"/>.
    /// </summary>
    /// <param name="name">Имя.</param>
    /// <param name="surname">Фамилия.</param>
    /// <param name="patronymic">Отчество.</param>
    /// <exception cref="DomainException">
    /// Если Имя, Фамилия пустое.
    /// Если Отчество превышает допустимый размер.
    /// </exception>
    public static FullName Of(string name, string surname, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"{nameof(Name)} не может быть пустым.");
        if (string.IsNullOrWhiteSpace(surname))
            throw new DomainException($"{nameof(Surname)} не может быть пустым.");

        name = Normalize(name);
        surname = Normalize(surname);
        patronymic = patronymic == null ? null : Normalize(patronymic);

        ValidatePart(name, nameof(Name));
        ValidatePart(surname, nameof(Surname));

        if (patronymic != null)
        {
            ValidatePart(patronymic, nameof(Patronymic));
        }

        return new FullName(name, surname, patronymic);
    }

    /// <summary>
    /// Валидация строки.
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="partName">Название свойства.</param>
    /// <exception cref="DomainException">
    /// Если значение пустое,
    /// Если привышает <see cref="MaxLength"/>,
    /// Если содержит недопустимые символы.
    /// </exception>
    private static void ValidatePart(string value, string partName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{partName} не может быть пустым.");
        }

        if (value.Length > MaxLength)
        {
            throw new DomainException($"{partName} не может превышать {MaxLength} символов.");
        }

        if (!Regex.IsMatch(value, NamePattern))
        {
            throw new DomainException($"{partName} содержит недопустимые символы. Разрешены только буквы и дефис.");
        }
    }

    /// <summary>
    /// Нормализовать строку.
    /// </summary>
    /// <param name="value">Значение.</param>
    private static string Normalize(string value) => value.Trim();

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Surname;
        yield return Patronymic;
    }

    /// <inheritdoc/>
    public int CompareTo(FullName? other)
    {
        if (other == null) return 1;
        var surnameCompare = string.Compare(Surname, other.Surname, StringComparison.Ordinal);
        if (surnameCompare != 0) return surnameCompare;
        var nameCompare = string.Compare(Name, other.Name, StringComparison.Ordinal);
        if (nameCompare != 0) return nameCompare;
        return string.Compare(Patronymic, other.Patronymic, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override string ToString() => $"{Surname} {Name} {Patronymic}".Trim();
}