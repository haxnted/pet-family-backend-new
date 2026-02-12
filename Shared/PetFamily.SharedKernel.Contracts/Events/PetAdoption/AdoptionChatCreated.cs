using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: чат усыновления успешно создан.
/// </summary>
public class AdoptionChatCreated : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор созданного чата.
    /// </summary>
    public required Guid ChatId { get; init; }

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public required Guid PetId { get; init; }
}
