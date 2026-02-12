using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: не удалось создать чат усыновления.
/// </summary>
public class AdoptionChatCreationFailed : CorrelatedBy<Guid>
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
    /// Причина сбоя создания чата.
    /// </summary>
    public required string Reason { get; init; }
}
