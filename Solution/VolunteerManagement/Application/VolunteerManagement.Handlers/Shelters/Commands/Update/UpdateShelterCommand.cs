using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.Update;

/// <summary>
/// Команда на обновление данных приюта.
/// </summary>
public sealed class UpdateShelterCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

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
