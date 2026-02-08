using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.HardRemoveAllPets;

/// <summary>
/// Команда на жёсткое удаление всех питомцев волонтёра.
/// </summary>
public sealed class HardRemoveAllPetsCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}
