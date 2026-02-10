using Ardalis.Specification;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Services.Specifications;

/// <summary>
/// Спецификация для поиска Аккаунта по идентификатору пользователя.
/// </summary>
public sealed class GetByUserIdSpecification : Specification<DomainAccount>
{
    /// <summary>
    /// Создаёт спецификацию для поиска аккаунта по UserId.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя из Auth.</param>
    public GetByUserIdSpecification(Guid userId)
    {
        Query.Where(account => account.UserId == userId);
    }
}
