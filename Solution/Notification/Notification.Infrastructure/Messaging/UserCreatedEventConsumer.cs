using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Application.Specifications;
using Notification.Core.Models;
using Notification.Infrastructure.Services.Email;
using PetFamily.SharedKernel.Contracts.Events.Auth;
using PetFamily.SharedKernel.Infrastructure.Abstractions;

namespace Notification.Infrastructure.Messaging;

/// <summary>
/// Consumer для события UserCreatedEvent из сервиса Auth.
/// Создаёт настройки уведомлений для пользователя и отправляет приветственное письмо.
/// </summary>
public class UserCreatedEventConsumer(
    IRepository<UserNotificationSettings> repository,
    IEmailService emailService,
    ILogger<UserCreatedEventConsumer> logger) : IConsumer<UserCreatedEvent>
{
    /// <inheritdoc />
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var @event = context.Message;

        logger.LogInformation(
            "Обработка UserCreatedEvent для пользователя {UserId} ({Email})",
            @event.UserId,
            @event.Email);

        try
        {
            var spec = new GetUserNotificationSettingsSpecification(@event.UserId);
            var existing = await repository.FirstOrDefaultAsync(spec, context.CancellationToken);

            if (existing == null)
            {
                var settings = new UserNotificationSettings
                {
                    UserId = @event.UserId,
                    IsMuted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    EmailSettings = new EmailSettings
                    {
                        Email = @event.Email,
                        IsEnabled = true
                    }
                };

                await repository.AddAsync(settings, context.CancellationToken);

                logger.LogInformation("Созданы базовые настройки для пользователя {UserId}", @event.UserId);
            }

            var subject = "Welcome to Pet Family!";
            var body = $"""
                        Здравствуй {(string.IsNullOrEmpty(@event.FirstName) ? "" : @event.FirstName)},

                        Добро пожаловать в Pet Family! Ваш аккаунт был успешно зарегистрирован.

                        С любовью,
                        Pet Family Team
                        """;

            await emailService.SendAsync(@event.Email, subject, body, context.CancellationToken);

            logger.LogInformation(
                "Welcome email sent to {Email}",
                @event.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to process UserCreatedEvent for user {UserId}",
                @event.UserId);
            throw;
        }
    }
}