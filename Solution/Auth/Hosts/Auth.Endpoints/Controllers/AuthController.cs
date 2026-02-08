using System.Security.Claims;
using Auth.Application.Services;
using Auth.Contracts.Dtos;
using Auth.Endpoints.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;


namespace Auth.Endpoints.Controllers;

/// <summary>
/// Контроллер для аутентификации и авторизации.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="request">Запрос на регистрацию пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthTokenResponse>> Register(
        [FromBody] UserRegisterRequest request,
        CancellationToken ct)
    {
        await authService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.Patronymic,
            ct);

        return NoContent();
    }

    /// <summary>
    /// Вход в систему.
    /// </summary>
    /// <param name="request">Запрос на вход.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthTokenResponse>> Login(
        [FromBody] UserLoginRequest request,
        CancellationToken ct)
    {
        var tokens = await authService.LoginAsync(request.Email, request.Password, ct);

        return Ok(tokens);
    }

    /// <summary>
    /// Обновление токена доступа.
    /// </summary>
    /// <param name="refreshToken">Refresh токен.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Новые токены доступа.</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthTokenResponse>> RefreshToken(
        [FromBody] string refreshToken,
        CancellationToken ct)
    {
        var tokens = await authService.RefreshTokenAsync(refreshToken, ct);

        return Ok(tokens);
    }

    /// <summary>
    /// Повторная отправка письма для подтверждения email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("resend-verification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ResendVerificationEmail(
        [FromBody] string email,
        CancellationToken ct)
    {
        await authService.ResendVerificationEmailAsync(email, ct);

        return Ok(new { message = "На вашу почту было отправлено письмо с подтверждение аккаунта." });
    }

    /// <summary>
    /// Отправка письма для восстановления пароля.
    /// </summary>
    /// <param name="request">Email пользователя.</param>
    /// <param name="ct">Токен отмены.</param>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken ct)
    {
        await authService.ForgotPasswordAsync(request.Email, ct);

        return NoContent();
    }

    /// <summary>
    /// Получить информацию о текущем пользователе.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Информация о пользователе.</returns>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid user token" });
        }

        var user = await authService.GetUserByIdAsync(userId, ct);

        if (user == null)
        {
            return NotFound(new { error = "User not found" });
        }

        return Ok(user);
    }
}