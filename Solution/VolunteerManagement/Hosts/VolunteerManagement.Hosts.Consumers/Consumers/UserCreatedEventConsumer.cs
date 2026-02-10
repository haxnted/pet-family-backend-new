using MassTransit;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using VolunteerManagement.Handlers.Volunteers.Commands.Add;

namespace VolunteerManagement.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для обработки события создания пользователя из Auth микросервиса.
/// Автоматически создает профиль волонтёра.
/// </summary>
public class UserCreatedEventConsumer(
    AddVolunteerHandler handler,
    ILogger<UserCreatedEventConsumer> logger)
    : IConsumer<UserCreatedEvent>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var userEvent = context.Message;

        logger.LogInformation(
            "Получено событие UserCreatedEvent для пользователя {UserId} ({Email})",
            userEvent.UserId,
            userEvent.Email);
        try
        {
            var command = new AddVolunteerCommand()
            {
                Name = userEvent.FirstName,
                Surname = userEvent.LastName,
                Patronymic = userEvent.Patronymic,
                UserId = userEvent.UserId
            };

            await handler.Handle(command, context.CancellationToken);

            logger.LogInformation("Успешно создан профиль волонтера для пользователя {UserId}", userEvent.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка при создании профиля волонтера для пользователя {UserId}",
                userEvent.UserId);

            throw;
        }
    }
}