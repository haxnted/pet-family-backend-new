using Account.Handlers.Commands.Create;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Contracts.Events.Auth;

namespace Account.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для обработки события создания пользователя из Auth микросервиса.
/// Автоматически создаёт профиль аккаунта.
/// </summary>
public class UserCreatedEventConsumer(
    CreateAccountHandler handler,
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
            var command = new CreateAccountCommand
            {
                UserId = userEvent.UserId
            };

            await handler.Handle(command, context.CancellationToken);

            logger.LogInformation("Успешно создан аккаунт для пользователя {UserId}", userEvent.UserId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Ошибка при создании аккаунта для пользователя {UserId}",
                userEvent.UserId);

            throw;
        }
    }
}
