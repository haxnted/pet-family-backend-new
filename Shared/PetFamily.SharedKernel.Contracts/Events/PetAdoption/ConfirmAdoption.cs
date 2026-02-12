using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда от волонтёра: подтвердить усыновление питомца.
/// </summary>
public class ConfirmAdoption : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра, подтверждающего усыновление.
    /// </summary>
    public required Guid VolunteerId { get; init; }
}
