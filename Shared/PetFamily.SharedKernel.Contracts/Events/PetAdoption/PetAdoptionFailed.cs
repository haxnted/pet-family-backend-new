using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: не удалось обновить статус питомца при усыновлении.
/// </summary>
public class PetAdoptionFailed : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Причина ошибки.
    /// </summary>
    public required string Reason { get; init; }
}
