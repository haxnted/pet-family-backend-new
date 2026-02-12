using Auth.Contracts.Dtos;
using Auth.Core.Models;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Contracts.Events.Auth;

namespace Auth.Application.Services;

/// <summary>
/// Реализация сервиса аутентификации.
/// </summary>
public class AuthService(
    AuthDbContext dbContext,
    IKeycloakService keycloakService,
    IPublishEndpoint publishEndpoint,
    ILogger<AuthService> logger)
    : IAuthService
{
    /// <inheritdoc/>
    public async Task RegisterAsync(string email,
        string password,
        string firstName,
        string lastName,
        string? patronymic,
        CancellationToken ct)
    {
        var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (existingUser != null)
        {
            throw new UseCaseException("Пользователь с таким email уже существует");
        }

        var userId = await keycloakService.CreateUserAsync(
            email,
            password,
            firstName,
            lastName,
            patronymic,
            ct);

        await keycloakService.AssignRoleToUserAsync(userId, "user", ct);

        var user = new User
        {
            Id = userId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Patronymic = patronymic,
            Role = "user",
            EmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(ct);

        var @event = new UserCreatedEvent(
            userId,
            email,
            firstName,
            lastName,
            patronymic,
            "user");

        await publishEndpoint.Publish(@event, ct);
        await dbContext.SaveChangesAsync(ct);

        try
        {
            await keycloakService.SendVerificationEmailAsync(userId, ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Failed to send verification email for user {UserId}. User can request resend later",
                userId);
        }
    }

    /// <inheritdoc/>
    public async Task<AuthTokenResponse> LoginAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        var tokens = await keycloakService.LoginAsync(email, password, ct);

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct) ?? throw new EntityNotFoundException<User>(email);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = tokens.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false,
            UserId = user.Id
        };

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(ct);

        return tokens;
    }

    /// <inheritdoc/>
    public async Task<AuthTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        var storedToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(rt =>
            rt.Token == refreshToken &&
            !rt.IsRevoked, ct);

        if (storedToken == null)
        {
            throw new EntityNotFoundException<User>(refreshToken);
        }

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UseCaseException("Refresh token истек");
        }

        var tokens = await keycloakService.RefreshTokenAsync(refreshToken, ct);

        storedToken.IsRevoked = true;

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = tokens.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false,
            UserId = storedToken.UserId
        };

        dbContext.RefreshTokens.Add(newRefreshToken);
        await dbContext.SaveChangesAsync(ct);

        return tokens;
    }

    /// <inheritdoc/>
    public async Task ResendVerificationEmailAsync(string email, CancellationToken ct)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user == null)
        {
            throw new EntityNotFoundException<User>(email);
        }

        if (user.EmailVerified)
        {
            throw new UseCaseException("Пользователь уже подтвержден");
        }

        await keycloakService.SendVerificationEmailAsync(user.Id, ct);
    }

    /// <inheritdoc/>
    public async Task ForgotPasswordAsync(string email, CancellationToken ct)
    {
        await keycloakService.SendPasswordResetEmailAsync(email, ct);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
        {
            return null;
        }

        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Patronymic,
            user.Role,
            user.EmailVerified,
            user.CreatedAt);
    }
}