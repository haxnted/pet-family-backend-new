using Account.Handlers.MappingExtensions;
using Account.Services;
using Account.Services.Caching;
using Account.Services.Dtos;
using PetFamily.SharedKernel.Infrastructure.Caching;

namespace Account.Handlers.Queries.GetByUserId;

/// <summary>
/// Обработчик запроса на получение аккаунта по идентификатору пользователя.
/// </summary>
public class GetAccountByUserIdHandler(IAccountService accountService, ICacheService cache)
{
    /// <summary>
    /// Обработать запрос на получение аккаунта.
    /// </summary>
    public async Task<AccountDto> Handle(GetAccountByUserIdQuery query, CancellationToken ct)
    {
        var cacheKey = CacheKeys.AccountByUserId(query.UserId);

        var cached = await cache.GetAsync<AccountDto>(cacheKey, ct);
        if (cached != null)
            return cached;

        var account = await accountService.GetByUserIdAsync(query.UserId, ct);

        var result = account.ToDto();

        await cache.SetAsync(cacheKey, result, ct, CacheDurations.Account);

        return result;
    }
}
