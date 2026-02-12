using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Services.Volunteers.Specifications;

namespace VolunteerManagement.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для компенсации: отмена бронирования питомца.
/// Если компенсация не удалась — только логируем, не бросаем исключение.
/// </summary>
public class UnreservePetConsumer(
    IRepository<Volunteer> repository,
    ILogger<UnreservePetConsumer> logger)
    : IConsumer<UnreservePet>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<UnreservePet> context)
    {
        var message = context.Message;

        logger.LogWarning(
            "Получена команда компенсации UnreservePet: PetId={PetId}, VolunteerId={VolunteerId}",
            message.PetId, message.VolunteerId);

        try
        {
            var spec = new GetByIdWithPetsSpecification(VolunteerId.Of(message.VolunteerId));
            var volunteer = await repository.FirstOrDefaultAsync(spec, context.CancellationToken);

            if (volunteer == null)
            {
                logger.LogWarning("Волонтёр {VolunteerId} не найден при компенсации", message.VolunteerId);
                await PublishUnreserved(context, message);
                return;
            }

            volunteer.CancelPetReservation(PetId.Of(message.PetId));
            await repository.UpdateAsync(volunteer, context.CancellationToken);

            logger.LogInformation("Компенсация: бронирование питомца {PetId} отменено", message.PetId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при компенсации бронирования питомца {PetId}", message.PetId);
        }

        await PublishUnreserved(context, message);
    }

    /// <summary>
    /// Публикация события <see cref="PetUnreserved"/> в шину.
    /// Вызывается всегда — независимо от результата компенсации.
    /// </summary>
    /// <param name="context">Контекст потребления.</param>
    /// <param name="message">Исходное сообщение.</param>
    private static Task PublishUnreserved(ConsumeContext<UnreservePet> context, UnreservePet message)
    {
        return context.Publish<PetUnreserved>(new
        {
            message.CorrelationId,
            message.PetId
        });
    }
}