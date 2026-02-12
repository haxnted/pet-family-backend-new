using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда от саги к VolunteerManagement: забронировать питомца.
/// </summary>
public class ReservePet : CorrelatedBy<Guid>
{
    /// <summary>
    /// Идентификатор корреляции саги.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Идентификатор питомца для бронирования.
    /// </summary>
    public required Guid PetId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра, ответственного за питомца.
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор усыновителя.
    /// </summary>
    public required Guid AdopterId { get; init; }
}
