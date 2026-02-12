using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: питомец успешно усыновлён (статус обновлён на FoundHome).
/// </summary>
public class PetAdopted : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор усыновлённого питомца.
    /// </summary>
    public required Guid PetId { get; init; }
}
