using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда от саги к Conversation: создать чат усыновления.
/// </summary>
public class CreateAdoptionChat : CorrelatedBy<Guid>
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
    /// Идентификатор волонтёра (участник чата).
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор усыновителя (участник чата).
    /// </summary>
    public required Guid AdopterId { get; init; }

    /// <summary>
    /// Кличка питомца (для заголовка чата).
    /// </summary>
    public required string PetNickName { get; init; }
}
