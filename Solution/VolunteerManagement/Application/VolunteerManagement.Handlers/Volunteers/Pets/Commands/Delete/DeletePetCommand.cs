using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Delete;

/// <summary>
/// Команда на удаление животного.
/// </summary>
public sealed class DeletePetCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор животного.
    /// </summary>
    public Guid PetId { get; init; }
}
