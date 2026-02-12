using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Domain.Aggregates.ValueObjects.Properties;

/// <summary>
/// Объект-значение фотография.
/// </summary>
public sealed class Photo : ValueObject, IComparable<Photo>
{
    /// <summary>
    /// Идентификатор файла в хранилище.
    /// </summary>
    public Guid Value { get; }

    private Photo(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Фабричный метод для создания фото <see cref="Photo"/>.
    /// </summary>
    /// <param name="fileId">Идентификатор файла в хранилище.</param>
    /// <returns>Фото <see cref="Photo"/>.</returns>
    /// <exception cref="DomainException">Если входные данные некорректны.</exception>
    public static Photo Create(Guid fileId)
    {
        if (fileId == Guid.Empty)
        {
            throw new DomainException("Идентификатор файла не может быть пустым.");
        }

        return new Photo(fileId);
    }

    /// <inheritdoc/>
    public int CompareTo(Photo? other)
    {
        if (other == null) return 1;
        return Value.CompareTo(other.Value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
