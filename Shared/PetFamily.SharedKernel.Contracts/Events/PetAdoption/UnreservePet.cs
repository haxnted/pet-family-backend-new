using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда компенсации: отменить бронирование питомца.
/// </summary>
public class UnreservePet : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор питомца для снятия бронирования.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра, ответственного за питомца.
    /// </summary>
    public required Guid VolunteerId { get; init; }
}
