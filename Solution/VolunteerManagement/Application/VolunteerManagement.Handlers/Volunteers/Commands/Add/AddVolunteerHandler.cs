using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.Add;

/// <summary>
/// Обработчик команды добавления волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class AddVolunteerHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду добавления волонтёра.
    /// </summary>
    /// <param name="command">Команда добавления волонтёра.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(AddVolunteerCommand command, CancellationToken ct)
    {
        await volunteerService.AddAsync(
            command.Name,
            command.Surname,
            command.Patronymic,
            command.UserId,
            command.GeneralDescription,
            ct);
    }
}