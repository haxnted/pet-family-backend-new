using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;

/// <summary>
/// Объект-значение Режим работы приюта.
/// </summary>
public sealed class WorkingHours : ValueObject, IComparable<WorkingHours>
{
    /// <summary>
    /// Время открытия.
    /// </summary>
    public TimeOnly OpenTime { get; }

    /// <summary>
    /// Время закрытия.
    /// </summary>
    public TimeOnly CloseTime { get; }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private WorkingHours(TimeOnly openTime, TimeOnly closeTime)
    {
        OpenTime = openTime;
        CloseTime = closeTime;
    }

    /// <summary>
    /// Фабричный метод для создания режима работы <see cref="WorkingHours"/>.
    /// </summary>
    /// <param name="openTime">Время открытия.</param>
    /// <param name="closeTime">Время закрытия.</param>
    /// <exception cref="DomainException">
    /// Если время закрытия раньше или равно времени открытия.
    /// </exception>
    public static WorkingHours Of(TimeOnly openTime, TimeOnly closeTime)
    {
        if (closeTime <= openTime)
        {
            throw new DomainException("Время закрытия должно быть позже времени открытия.");
        }

        return new WorkingHours(openTime, closeTime);
    }

    /// <inheritdoc/>
    public int CompareTo(WorkingHours? other)
    {
        if (other == null) return 1;
        var openComparison = OpenTime.CompareTo(other.OpenTime);
        return openComparison != 0
            ? openComparison
            : CloseTime.CompareTo(other.CloseTime);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OpenTime;
        yield return CloseTime;
    }
}
