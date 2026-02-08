using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Volunteers.Commands.Update;

/// <summary>
/// Команда на обновление данных волонтёра.
/// </summary>
public sealed class UpdateVolunteerCommand : Command
{
    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; init; }

    /// <summary>
    /// Описание волонтёра.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Опыт работы волонтёром в годах.
    /// </summary>
    public int? AgeExperience { get; init; }

    /// <summary>
    /// Контактный номер телефона.
    /// </summary>
    public string? PhoneNumber { get; init; }
}
