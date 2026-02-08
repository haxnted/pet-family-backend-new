using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.SoftDelete;

/// <summary>
/// Команда на мягкое удаление питомца.
/// </summary>
public sealed class SoftDeletePetCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public Guid PetId { get; init; }
}
