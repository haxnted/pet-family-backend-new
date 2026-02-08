using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteSpecies;

/// <summary>
/// Команда на удаление вида животного.
/// </summary>
public sealed class DeleteSpeciesCommand : Command
{
    /// <summary>
    /// Идентификатор вида животного.
    /// </summary>
    public Guid SpeciesId { get; init; }
}
