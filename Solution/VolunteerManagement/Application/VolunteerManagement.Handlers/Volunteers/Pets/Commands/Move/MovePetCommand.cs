using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.Move;

/// <summary>
/// Команда на перемещение питомца на новую позицию.
/// </summary>
public sealed class MovePetCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public Guid PetId { get; init; }

    /// <summary>
    /// Новая позиция.
    /// </summary>
    public int NewPosition { get; init; }
}
