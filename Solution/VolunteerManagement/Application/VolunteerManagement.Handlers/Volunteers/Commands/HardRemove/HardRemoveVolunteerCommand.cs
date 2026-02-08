using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.HardRemove;

/// <summary>
/// Команда на жёсткое удаление волонтёра.
/// </summary>
public sealed class HardRemoveVolunteerCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}
