using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для обработки команды бронирования питомца (шаг саги).
/// </summary>
public class ReservePetConsumer(
    IRepository<Volunteer> repository,
    ILogger<ReservePetConsumer> logger)
    : IConsumer<ReservePet>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<ReservePet> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Получена команда ReservePet: PetId={PetId}, VolunteerId={VolunteerId}, AdopterId={AdopterId}",
            message.PetId, message.VolunteerId, message.AdopterId);

        try
        {
            var spec = new GetByIdWithPetsSpecification(VolunteerId.Of(message.VolunteerId));
            var volunteer = await repository.FirstOrDefaultAsync(spec, context.CancellationToken)
                            ?? throw new InvalidOperationException(
                                $"Волонтёр {message.VolunteerId} не найден.");

            volunteer.ReservePet(PetId.Of(message.PetId), message.AdopterId);

            await repository.UpdateAsync(volunteer, context.CancellationToken);

            await context.Publish<PetReserved>(new
            {
                message.CorrelationId,
                message.PetId,
                message.VolunteerId,
                message.AdopterId
            });

            logger.LogInformation("Питомец {PetId} успешно забронирован", message.PetId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при бронировании питомца {PetId}", message.PetId);

            await context.Publish<PetReservationFailed>(new
            {
                message.CorrelationId,
                message.PetId,
                Reason = ex.Message
            });
        }
    }
}