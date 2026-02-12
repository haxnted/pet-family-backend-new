using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда от саги к VolunteerManagement: перевести питомца в статус "нашёл дом".
/// </summary>
public class AdoptPet : CorrelatedBy<Guid>
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
    /// Идентификатор волонтёра, ответственного за питомца.
    /// </summary>
    public required Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор усыновителя.
    /// </summary>
    public required Guid AdopterId { get; init; }
}
