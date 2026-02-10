namespace Account.Handlers.Queries.GetByUserId;

/// <summary>
/// Запрос на получение аккаунта по идентификатору пользователя.
/// </summary>
/// <param name="UserId">Идентификатор пользователя из Auth.</param>
public sealed record GetAccountByUserIdQuery(Guid UserId);
