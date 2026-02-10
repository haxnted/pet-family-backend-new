using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.HardRemove;

/// <summary>
/// Команда на полное удаление приюта.
/// </summary>
public sealed class HardRemoveShelterCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }
}
