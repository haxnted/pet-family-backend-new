using MassTransit;

namespace PetFamily.SharedKernel.Contracts.Events.PetAdoption;

/// <summary>
/// Команда инициации саги усыновления питомца.
/// </summary>
public class StartPetAdoption : CorrelatedBy<Guid>
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

    /// <summary>
    /// Имя усыновителя.
    /// </summary>
    public required string AdopterName { get; init; }

    /// <summary>
    /// Кличка питомца.
    /// </summary>
    public required string PetNickName { get; init; }
}
