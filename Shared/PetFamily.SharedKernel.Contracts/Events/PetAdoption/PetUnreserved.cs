using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: бронирование питомца успешно отменено (компенсация).
/// </summary>
public class PetUnreserved : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор питомца, бронирование которого отменено.
    /// </summary>
    public required Guid PetId { get; init; }
}
