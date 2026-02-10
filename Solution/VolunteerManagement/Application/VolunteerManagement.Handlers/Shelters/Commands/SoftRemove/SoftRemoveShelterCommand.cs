using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.SoftRemove;

/// <summary>
/// Команда на мягкое удаление приюта.
/// </summary>
public sealed class SoftRemoveShelterCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }
}
