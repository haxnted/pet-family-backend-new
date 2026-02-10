using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.Add;

/// <summary>
/// Команда на добавление Волонтёра.
/// </summary>
public sealed class AddVolunteerCommand : Command
{
    /// <summary>
    /// Имя.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Фамилия.
    /// </summary>
    public required string Surname { get; init; }

    /// <summary>
    /// Отчество.
    /// </summary>
    public string? Patronymic { get; init; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; init; }
}