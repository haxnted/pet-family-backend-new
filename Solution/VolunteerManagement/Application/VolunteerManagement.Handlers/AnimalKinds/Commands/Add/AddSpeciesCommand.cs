using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.AnimalKinds.Commands.Add;

/// <summary>
/// Команда на добавление вида животного.
/// </summary>
public sealed class AddSpeciesCommand : Command
{
    /// <summary>
    /// Вид животного.
    /// </summary>
    public string AnimalKind { get; init; } = string.Empty;
}
