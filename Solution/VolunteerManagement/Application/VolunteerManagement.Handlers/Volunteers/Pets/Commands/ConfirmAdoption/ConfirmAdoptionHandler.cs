using VolunteerManagement.Services.Volunteers.Adoption;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.ConfirmAdoption;

/// <summary>
/// Обработчик команды на подтверждение усыновления.
/// </summary>
public class ConfirmAdoptionHandler(IPetAdoptionService adoptionService)
{
    /// <summary>
    /// Обработать команду.
    /// </summary>
    public async Task Handle(ConfirmAdoptionCommand command, CancellationToken ct)
    {
        await adoptionService.ConfirmAsync(command.SagaId, command.CurrentUserId, ct);
    }
}
