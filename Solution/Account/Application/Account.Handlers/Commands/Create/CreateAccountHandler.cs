using Account.Services;

namespace Account.Handlers.Commands.Create;

/// <summary>
/// Обработчик команды создания аккаунта.
/// </summary>
/// <param name="accountService">Сервис для работы с аккаунтами.</param>
public class CreateAccountHandler(IAccountService accountService)
{
    /// <summary>
    /// Обрабатывает команду создания аккаунта.
    /// </summary>
    /// <param name="command">Команда создания аккаунта.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public async Task Handle(CreateAccountCommand command, CancellationToken ct)
    {
        await accountService.CreateAsync(command.UserId, ct);
    }
}
