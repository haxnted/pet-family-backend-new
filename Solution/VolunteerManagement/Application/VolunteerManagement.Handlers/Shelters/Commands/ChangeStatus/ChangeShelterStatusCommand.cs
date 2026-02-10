using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.ChangeStatus;

/// <summary>
/// Команда на изменение статуса приюта.
/// </summary>
public sealed class ChangeShelterStatusCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }

    /// <summary>
    /// Новый статус (0 = Active, 1 = TemporaryClosed, 2 = Inactive).
    /// </summary>
    public int NewStatus { get; init; }
}
