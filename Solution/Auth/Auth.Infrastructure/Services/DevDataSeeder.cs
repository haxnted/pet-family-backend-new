using Auth.Core.Models;
using Auth.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.SharedKernel.Contracts.Events.Auth;

namespace Auth.Infrastructure.Services;

/// <summary>
/// Сидер тестовых пользователей для локальной разработки.
/// Создаёт 3 пользователя с ролями admin, volunteer, user при первом запуске.
/// </summary>
public class DevDataSeeder(
    AuthDbContext dbContext,
    IKeycloakService keycloakService,
    IPublishEndpoint publishEndpoint,
    ILogger<DevDataSeeder> logger)
{
    private record SeedUser(string Email, string Password, string FirstName, string LastName, string Role);

    private static readonly SeedUser[] SeedUsers =
    [
        new("admin@petfamily.local", "Admin123!", "Admin", "Adminov", "admin"),
        new("volunteer@petfamily.local", "Volunteer123!", "Volunteer", "Volunteerov", "volunteer"),
        new("user@petfamily.local", "User123!", "User", "Userov", "user")
    ];

    /// <summary>
    /// Создать тестовых пользователей, если они ещё не существуют.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    public async Task SeedAsync(CancellationToken ct)
    {
        foreach (var seedUser in SeedUsers)
        {
            try
            {
                var exists = await dbContext.Users.AnyAsync(u => u.Email == seedUser.Email, ct);
                if (exists)
                    continue;

                var userId = await keycloakService.CreateUserAsync(
                    seedUser.Email,
                    seedUser.Password,
                    seedUser.FirstName,
                    seedUser.LastName,
                    null,
                    ct);

                await keycloakService.AssignRoleToUserAsync(userId, seedUser.Role, ct);

                var user = new User
                {
                    Id = userId,
                    Email = seedUser.Email,
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName,
                    Role = seedUser.Role,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync(ct);

                await publishEndpoint.Publish(
                    new UserCreatedEvent(
                        userId,
                        seedUser.Email,
                        seedUser.FirstName,
                        seedUser.LastName,
                        null,
                        seedUser.Role),
                    ct);

                await dbContext.SaveChangesAsync(ct);

                logger.LogInformation(
                    "Создан тестовый пользователь {Email} с ролью {Role} (ID: {UserId})",
                    seedUser.Email, seedUser.Role, userId);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "Не удалось создать тестового пользователя {Email}: {Message}",
                    seedUser.Email, ex.Message);
            }
        }
    }
}