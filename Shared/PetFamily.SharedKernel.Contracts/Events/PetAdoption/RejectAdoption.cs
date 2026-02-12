using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда от волонтёра: отклонить усыновление питомца.
/// </summary>
public class RejectAdoption : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра, отклоняющего усыновление.
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Причина отклонения.
    /// </summary>
    public string? Reason { get; init; }
}
