using PetFamily.SharedKernel.Domain.Primitives;

namespace VolunteerManagement.Handlers.Shelters.Commands.UpdateAddress;

/// <summary>
/// Команда на обновление адреса приюта.
/// </summary>
public sealed class UpdateShelterAddressCommand : Command
{
    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid ShelterId { get; init; }

    /// <summary>
    /// Улица.
    /// </summary>
    public required string Street { get; init; }

    /// <summary>
    /// Город.
    /// </summary>
    public required string City { get; init; }

    /// <summary>
    /// Регион.
    /// </summary>
    public required string State { get; init; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public required string ZipCode { get; init; }
}
