using Conversation.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Contracts.Events.PetAdoption;

namespace Conversation.Hosts.Consumers.Consumers;

/// <summary>
/// Consumer для создания чата усыновления (шаг саги).
/// </summary>
public class CreateAdoptionChatConsumer(
    IChatService chatService,
    ILogger<CreateAdoptionChatConsumer> logger)
    : IConsumer<CreateAdoptionChat>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<CreateAdoptionChat> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Получена команда CreateAdoptionChat: PetId={PetId}, PetNickName={PetNickName}",
            message.PetId, message.PetNickName);

        try
        {
            var chatId = await chatService.CreateAsync(
                title: $"Усыновление: {message.PetNickName}",
                description: $"Чат между волонтёром и усыновителем по питомцу {message.PetNickName}",
                linkedId: message.PetId,
                context.CancellationToken);

            await context.Publish<AdoptionChatCreated>(new
            {
                message.CorrelationId,
                ChatId = chatId,
                message.PetId
            });

            logger.LogInformation("Чат усыновления {ChatId} создан для питомца {PetId}", chatId, message.PetId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании чата усыновления для питомца {PetId}", message.PetId);

            await context.Publish<AdoptionChatCreationFailed>(new
            {
                message.CorrelationId,
                message.PetId,
                Reason = ex.Message
            });
        }
    }
}