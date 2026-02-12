using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: питомец успешно забронирован.
/// </summary>
public class PetReserved : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор забронированного питомца.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор усыновителя.
    /// </summary>
    public required Guid AdopterId { get; init; }
}
