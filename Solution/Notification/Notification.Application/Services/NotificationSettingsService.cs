using Notification.Application.Dtos;
using Notification.Application.MappingExtensions;
using Notification.Application.Specifications;
using Notification.Core.Models;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Infrastructure.Abstractions;

namespace Notification.Application.Services;

/// <inheritdoc/>
public class NotificationSettingsService(
    IRepository<UserNotificationSettings> repository)
    : INotificationSettingsService
{
    /// <inheritdoc/>
    public async Task DisableNotifyAsync(Guid userId, CancellationToken ct)
    {
        var specification = new GetUserNotificationSettingsSpecification(userId);

        var configuration = await repository.FirstOrDefaultAsync(specification, ct);

        if (configuration == null)
        {
            throw new EntityNotFoundException<UserNotificationSettings>(userId);
        }

        configuration.IsMuted = true;

        await repository.UpdateAsync(configuration, ct);
    }

    /// <inheritdoc/>
    public async Task<UserNotificationSettingsDto> GetSettingsByIdAsync(Guid userId, CancellationToken ct)
    {
        var specification = new GetUserNotificationSettingsWithIncludesSpecification(userId);

        var configuration = await repository.FirstOrDefaultAsync(specification, ct);

        if (configuration == null)
        {
            throw new EntityNotFoundException<UserNotificationSettings>(userId);
        }

        var mappedConfiguration = configuration.ToDto();
        return mappedConfiguration;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Guid userId, bool isEmailNotifyEnabled, CancellationToken ct)
    {
        var specification = new GetUserNotificationSettingsSpecification(userId);

        var configuration = await repository.FirstOrDefaultAsync(specification, ct);

        if (configuration == null)
        {
            throw new EntityNotFoundException<UserNotificationSettings>(userId);
        }

        if (configuration.EmailSettings == null)
        {
            configuration.EmailSettings = new EmailSettings
            {
                Id = Guid.NewGuid(),
                UserNotificationSettingsId = configuration.Id,
                IsEnabled = isEmailNotifyEnabled,
                UserNotificationSettings = configuration
            };
        }
        else
        {
            configuration.EmailSettings.IsEnabled = isEmailNotifyEnabled;
        }

        configuration.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(configuration, ct);
    }

    /// <inheritdoc/>
    public async Task CreateConfigurationAsync(Guid userId, CancellationToken ct)
    {
        var specification = new GetUserNotificationSettingsSpecification(userId);

        var configuration = await repository.FirstOrDefaultAsync(specification, ct);

        if (configuration != null)
        {
            throw new UseCaseException("Настройки уведомлений для данного пользователя уже существуют.");
        }

        var newConfiguration = new UserNotificationSettings
        {
            UserId = userId,
            IsMuted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            EmailSettings = new EmailSettings
            {
                Email = string.Empty,
                IsEnabled = false
            }
        };

        await repository.AddAsync(newConfiguration, ct);
    }
}