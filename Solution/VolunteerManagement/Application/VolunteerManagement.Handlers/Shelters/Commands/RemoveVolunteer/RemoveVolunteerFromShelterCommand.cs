using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.RemoveVolunteer;

/// <summary>
/// Команда на удаление волонтёра из приюта.
/// </summary>
public sealed class RemoveVolunteerFromShelterCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }

    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}
