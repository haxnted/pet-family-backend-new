using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.ActivateAccount;

/// <summary>
/// Команда на активацию аккаунта волонтёра.
/// </summary>
public sealed class ActivateAccountVolunteerCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }
}