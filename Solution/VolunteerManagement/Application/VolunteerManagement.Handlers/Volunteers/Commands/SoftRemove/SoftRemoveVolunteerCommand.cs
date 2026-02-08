using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.SoftRemove;

/// <summary>
/// Команда на мягкое удаление волонтёра.
/// </summary>
public sealed class SoftRemoveVolunteerCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}
