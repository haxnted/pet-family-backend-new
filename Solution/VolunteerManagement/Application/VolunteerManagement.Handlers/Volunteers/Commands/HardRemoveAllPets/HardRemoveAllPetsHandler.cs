using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.HardRemoveAllPets;

/// <summary>
/// Обработчик команды жёсткого удаления всех питомцев волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
public class HardRemoveAllPetsHandler(IVolunteerService volunteerService)
{
    /// <summary>
    /// Обрабатывает команду жёсткого удаления всех питомцев.
    /// </summary>
    /// <param name="command">Команда удаления питомцев.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(HardRemoveAllPetsCommand command, CancellationToken ct)
    {
        await volunteerService.HardRemoveAllPetsAsync(command.VolunteerId, ct);
    }
}