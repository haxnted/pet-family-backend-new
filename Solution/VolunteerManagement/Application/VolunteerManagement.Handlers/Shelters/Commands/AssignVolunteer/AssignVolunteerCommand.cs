using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.AssignVolunteer;

/// <summary>
/// Команда на назначение волонтёра в приют.
/// </summary>
public sealed class AssignVolunteerCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Роль волонтёра (0 = Manager, 1 = Caretaker, 2 = Veterinarian).
    /// </summary>
    public int Role { get; init; }
}
