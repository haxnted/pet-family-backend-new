using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreBreed;

/// <summary>
/// Команда на восстановление породы.
/// </summary>
public sealed class RestoreBreedCommand : Command
{
    /// <summary>
    /// Идентификатор вида.
    /// </summary>
    public Guid SpeciesId { get; init; }

    /// <summary>
    /// Идентификатор породы.
    /// </summary>
    public Guid BreedId { get; init; }
}
