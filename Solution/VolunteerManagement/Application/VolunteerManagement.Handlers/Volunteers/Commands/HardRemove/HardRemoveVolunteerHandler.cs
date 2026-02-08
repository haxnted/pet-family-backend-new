using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.HardRemove;

/// <summary>
/// Обработчик команды жёсткого удаления волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class HardRemoveVolunteerHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду жёсткого удаления волонтёра.
    /// </summary>
    /// <param name="command">Команда жёсткого удаления.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(HardRemoveVolunteerCommand command, CancellationToken ct)
    {
        await volunteerService.HardRemoveAsync(command.VolunteerId, ct);
    }
}
