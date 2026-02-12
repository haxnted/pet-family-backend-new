using VolunteerManagement.Services.Volunteers.Adoption;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.RejectAdoption;

/// <summary>
/// Обработчик команды на отклонение усыновления.
/// </summary>
public class RejectAdoptionHandler(IPetAdoptionService adoptionService)
{
    /// <summary>
    /// Обработать команду.
    /// </summary>
    public async Task Handle(RejectAdoptionCommand command, CancellationToken ct)
    {
        await adoptionService.RejectAsync(command.SagaId, command.CurrentUserId, command.Reason, ct);
    }
}
