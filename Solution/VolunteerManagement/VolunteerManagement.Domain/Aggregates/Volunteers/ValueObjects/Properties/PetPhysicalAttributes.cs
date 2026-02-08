using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

/// <summary>
/// Объект-значение Физические показатели животного.
/// </summary>
public sealed class PetPhysicalAttributes : ValueObject, IComparable<PetPhysicalAttributes>
{
    /// <summary>
    /// Вес.
    /// </summary>
    public double Weight { get; }

    /// <summary>
    /// Рост.
    /// </summary>
    public double Height { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="weight">Вес.</param>
    /// <param name="height">Высота.</param>
    private PetPhysicalAttributes(double weight, double height)
    {
        Weight = weight;
        Height = height;
    }

    /// <summary>
    /// Фабричный метод для создания физических показателей животного.
    /// </summary>
    /// <param name="weight">Вес.</param>
    /// <param name="height">Рост.</param>
    /// <exception cref="DomainException">
    /// Если вес или рост меньше либо равен нулю.
    /// </exception>
    public static PetPhysicalAttributes Of(double weight, double height)
    {
        if (weight <= 0)
            throw new DomainException("Вес животного не может быть меньше или равен 0.");

        if (height <= 0)
            throw new DomainException("Рост животного не может быть меньше или равен 0.");

        return new PetPhysicalAttributes(weight, height);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Weight;
        yield return Height;
    }

    /// <inheritdoc/>
    public int CompareTo(PetPhysicalAttributes? other)
    {
        if (other == null) return 1;
        var w = Weight.CompareTo(other.Weight);
        return w != 0 ? w : Height.CompareTo(other.Height);
    }
}