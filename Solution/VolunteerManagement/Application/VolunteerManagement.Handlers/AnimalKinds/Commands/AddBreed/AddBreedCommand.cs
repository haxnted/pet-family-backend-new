using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.AddBreed;

/// <summary>
/// Команда на добавление породы к виду животного.
/// </summary>
public sealed class AddBreedCommand : Command
{
    /// <summary>
    /// Идентификатор вида животного.
    /// </summary>
    public Guid SpeciesId { get; init; }

    /// <summary>
    /// Название породы.
    /// </summary>
    public string BreedName { get; init; } = string.Empty;
}
