using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using VolunteerManagement.Services.Volunteers.Pets;

namespace VolunteerManagement.Handlers.Volunteers.Pets.Commands.InitiateAdoption;

/// <summary>
/// Обработчик команды на инициацию усыновления питомца.
/// Получает данные питомца через сервис и публикует событие запуска саги.
/// </summary>
public class InitiateAdoptionHandler(IPetService petService, IBus massTransitBus)
{
    /// <summary>
    /// Обработать команду на инициацию усыновления.
    /// </summary>
    /// <returns>Идентификатор созданной саги.</returns>
    public async Task<Guid> Handle(InitiateAdoptionCommand command, CancellationToken ct)
    {
        var pet = await petService.GetPetById(command.VolunteerId, command.PetId, ct);

        var sagaId = Guid.NewGuid();

        await massTransitBus.Publish(new StartPetAdoption
        {
            CorrelationId = sagaId,
            PetId = command.PetId,
            VolunteerId = command.VolunteerId,
            AdopterId = command.AdopterId,
            AdopterName = command.AdopterName,
            PetNickName = pet.NickName.Value
        }, ct);

        return sagaId;
    }
}
