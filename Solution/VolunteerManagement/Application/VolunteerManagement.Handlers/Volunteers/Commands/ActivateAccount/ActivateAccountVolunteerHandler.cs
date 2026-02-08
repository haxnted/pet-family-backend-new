using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.ActivateAccount;

/// <summary>
/// Обработчик команды активации аккаунта волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class ActivateAccountVolunteerHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду активации аккаунта волонтёра.
    /// </summary>
    /// <param name="command">Команда активации аккаунта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(ActivateAccountVolunteerCommand command, CancellationToken ct)
    {
        await volunteerService.ActivateAccountVolunteerRequest(command.VolunteerId, ct);
    }
}