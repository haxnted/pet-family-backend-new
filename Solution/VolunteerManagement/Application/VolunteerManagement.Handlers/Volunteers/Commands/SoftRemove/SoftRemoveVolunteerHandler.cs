using PetFamily.SharedKernel.Infrastructure.Caching;
using VolunteerManagement.Services.Caching;
using VolunteerManagement.Services.Volunteers;

namespace VolunteerManagement.Handlers.Volunteers.Commands.SoftRemove;

/// <summary>
/// Обработчик команды мягкого удаления волонтёра.
/// </summary>
/// <param name="volunteerService">Сервис для работы с волонтёрами.</param>
/// <param name="cache">Сервис кэширования.</param>
public class SoftRemoveVolunteerHandler(IVolunteerService volunteerService, ICacheService cache)
{
	/// <summary>
	/// Обрабатывает команду мягкого удаления волонтёра.
	/// </summary>
	/// <param name="command">Команда мягкого удаления.</param>
	/// <param name="ct">Токен отмены операции.</param>
	public async Task Handle(SoftRemoveVolunteerCommand command, CancellationToken ct)
	{
		await volunteerService.SoftRemoveAsync(command.VolunteerId, ct);

		await cache.RemoveAsync(CacheKeys.VolunteerById(command.VolunteerId), ct);
	}
}
