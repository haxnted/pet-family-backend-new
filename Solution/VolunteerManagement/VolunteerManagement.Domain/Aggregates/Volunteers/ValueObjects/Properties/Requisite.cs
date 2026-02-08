using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение Реквизит.
/// </summary>
public sealed class Requisite : ValueObject, IComparable<Requisite>
{
    /// <summary>
    /// Название реквизита.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Описание реквизита.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="name">Название реквизита.</param>
    /// <param name="description">Описание реквизита.</param>
    private Requisite(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Фабричный метод для создания объекта <see cref="Requisite"/>.
    /// </summary>
    /// <param name="name">Название реквизита.</param>
    /// <param name="description">Описание реквизита.</param>
    /// <exception cref="DomainException">
    /// Если название или описание некорректны.
    /// </exception>
    public static Requisite Of(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Название реквизита не может быть пустым.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Описание реквизита не может быть пустым.");

        return new Requisite(name.Trim(), description.Trim());
    }


    /// <inheritdoc/>
    public int CompareTo(Requisite? other)
    {
        if (other == null) return 1;
        var nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
        return nameComparison != 0
            ? nameComparison
            : string.Compare(Description, other.Description, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}