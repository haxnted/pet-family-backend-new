using Account.Services;

namespace Account.Handlers.Commands.UpdatePhoto;

/// <summary>
/// Обработчик команды обновления фотографии профиля.
/// </summary>
/// <param name="accountService">Сервис для работы с аккаунтами.</param>
public class UpdatePhotoHandler(IAccountService accountService)
{
    /// <summary>
    /// Обрабатывает команду обновления фотографии.
    /// </summary>
    /// <param name="command">Команда обновления фотографии.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(UpdatePhotoCommand command, CancellationToken ct)
    {
        await accountService.UpdatePhotoAsync(
            command.UserId,
            command.PhotoId,
            ct);
    }
}
