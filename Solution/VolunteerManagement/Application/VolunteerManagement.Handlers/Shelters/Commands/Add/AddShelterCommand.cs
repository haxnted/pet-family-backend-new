using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.Add;

/// <summary>
/// Команда на добавление Приюта.
/// </summary>
public sealed class AddShelterCommand : Command
{
    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Улица.
    /// </summary>
    public required string Street { get; init; }

    /// <summary>
    /// Город.
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Регион.
    /// </summary>
    public required string State { get; init; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public required string ZipCode { get; init; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public required string PhoneNumber { get; init; }

    /// <summary>
    /// Описание.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Время открытия.
    /// </summary>
    public TimeOnly OpenTime { get; init; }

    /// <summary>
    /// Время закрытия.
    /// </summary>
    public TimeOnly CloseTime { get; init; }

    /// <summary>
    /// Вместимость.
    /// </summary>
    public int Capacity { get; init; }
}
