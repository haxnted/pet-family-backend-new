using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Handlers.Commands.Create;

/// <summary>
/// Команда на создание аккаунта.
/// </summary>
public sealed class CreateAccountCommand : Command
{
    /// <summary>
    /// Идентификатор пользователя из Auth.
    /// </summary>
    public required Guid UserId { get; init; }
}
