using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Dtos;
using Notification.Application.Services;
using PetFamily.SharedKernel.WebApi.Services;

namespace Notification.Hosts.Endpoints.Controllers;

/// <summary>
/// Контроллер для работы с настройками уведомлений.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController(
    ICurrentUser user,
    INotificationSettingsService settingsService) : ControllerBase
{
    /// <summary>
    /// Получает настройки уведомлений текущего пользователя.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    [HttpGet]
    [ProducesResponseType(typeof(UserNotificationSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserNotificationSettingsDto>> Get(CancellationToken ct)
    {
        var userId = user.UserId;

        var result = await settingsService.GetSettingsByIdAsync(userId, ct);

        return Ok(result);
    }

    /// <summary>
    /// Создаёт настройки уведомлений для текущего пользователя.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserNotificationSettingsDto>> Create(CancellationToken ct)
    {
        var userId = user.UserId;

        await settingsService.CreateConfigurationAsync(userId, ct);

        return NoContent();
    }

    /// <summary>
    /// Обновляет настройки уведомлений пользователя.
    /// </summary>
    /// <param name="isEmailNotifyEnabled">Флаг, указывающий, включены ли email-уведомления.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("notify")]
    [ProducesResponseType(typeof(UserNotificationSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserNotificationSettingsDto>> Update(
        [FromBody] bool isEmailNotifyEnabled,
        CancellationToken ct)
    {
        var userId = user.UserId;

        await settingsService.UpdateAsync(userId, isEmailNotifyEnabled, ct);

        return NoContent();
    }

    /// <summary>
    /// Отключает все уведомления для текущего пользователя.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    [HttpPatch("all")]
    [ProducesResponseType(typeof(UserNotificationSettingsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserNotificationSettingsDto>> DisableNotify(CancellationToken ct)
    {
        var userId = user.UserId;

        await settingsService.DisableNotifyAsync(userId, ct);

        return NoContent();
    }
}
