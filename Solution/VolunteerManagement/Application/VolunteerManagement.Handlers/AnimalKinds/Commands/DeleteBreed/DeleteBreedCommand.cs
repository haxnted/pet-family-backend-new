using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.DeleteBreed;

/// <summary>
/// Команда на удаление породы.
/// </summary>
public sealed class DeleteBreedCommand : Command
{
    /// <summary>
    /// Идентификатор вида животного.
    /// </summary>
    public Guid SpeciesId { get; init; }

    /// <summary>
    /// Идентификатор породы.
    /// </summary>
    public Guid BreedId { get; init; }
}
