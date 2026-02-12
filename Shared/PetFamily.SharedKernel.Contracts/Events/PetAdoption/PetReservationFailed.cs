using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Событие: не удалось забронировать питомца.
/// </summary>
public class PetReservationFailed : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор питомца, бронирование которого не удалось.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Причина сбоя бронирования.
    /// </summary>
    public required string Reason { get; init; }
}
