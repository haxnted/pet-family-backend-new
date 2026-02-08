using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.RestoreSpecies;

/// <summary>
/// Команда на восстановление вида животного.
/// </summary>
public sealed class RestoreSpeciesCommand : Command
{
    /// <summary>
    /// Идентификатор вида.
    /// </summary>
    public Guid SpeciesId { get; init; }
}
