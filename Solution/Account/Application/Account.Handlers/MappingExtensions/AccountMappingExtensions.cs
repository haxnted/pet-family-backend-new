using Account.Services.Dtos;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Handlers.MappingExtensions;

/// <summary>
/// Методы расширения для маппинга Account в DTO.
/// </summary>
public static class AccountMappingExtensions
{
    /// <summary>
    /// Преобразовать сущность Account в DTO.
    /// </summary>
    /// <param name="account">Сущность аккаунта.</param>
    /// <returns>DTO аккаунта.</returns>
    public static AccountDto ToDto(this DomainAccount account)
    {
        return new AccountDto(
            account.Id.Value,
            account.UserId,
            account.PhoneNumber?.Value,
            account.AgeExperience?.Value,
            account.Description?.Value,
            account.Photo?.Value);
    }
}
