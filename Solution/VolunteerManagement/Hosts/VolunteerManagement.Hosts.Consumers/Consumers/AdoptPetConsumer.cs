using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для обработки команды усыновления питомца (шаг саги).
/// </summary>
public class AdoptPetConsumer(
    IRepository<Volunteer> repository,
    ILogger<AdoptPetConsumer> logger)
    : IConsumer<AdoptPet>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<AdoptPet> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Получена команда AdoptPet: PetId={PetId}, VolunteerId={VolunteerId}, AdopterId={AdopterId}",
            message.PetId, message.VolunteerId, message.AdopterId);

        try
        {
            var spec = new GetByIdWithPetsSpecification(VolunteerId.Of(message.VolunteerId));
            var volunteer = await repository.FirstOrDefaultAsync(spec, context.CancellationToken)
                            ?? throw new InvalidOperationException(
                                $"Волонтёр {message.VolunteerId} не найден.");

            volunteer.AdoptPet(PetId.Of(message.PetId));

            await repository.UpdateAsync(volunteer, context.CancellationToken);

            await context.Publish<PetAdopted>(new
            {
                message.CorrelationId,
                message.PetId
            });

            logger.LogInformation("Питомец {PetId} успешно усыновлён", message.PetId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при усыновлении питомца {PetId}", message.PetId);

            await context.Publish<PetAdoptionFailed>(new
            {
                message.CorrelationId,
                message.PetId,
                Reason = ex.Message
            });
        }
    }
}