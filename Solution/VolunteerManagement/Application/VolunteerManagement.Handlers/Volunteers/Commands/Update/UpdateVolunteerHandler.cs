using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.Update;

/// <summary>
/// Обработчик команды обновления данных волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class UpdateVolunteerHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду обновления данных волонтёра.
    /// </summary>
    /// <param name="command">Команда обновления волонтёра.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdateVolunteerCommand command, CancellationToken ct)
    {
        await volunteerService.UpdateAsync(
            command.VolunteerId,
            command.Description,
            command.AgeExperience,
            command.PhoneNumber,
            ct);
    }
}
