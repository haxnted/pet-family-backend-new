using Account.Domain.Aggregates.ValueObjects;
using Account.Domain.Aggregates.ValueObjects.Identifiers;
using Account.Domain.Aggregates.ValueObjects.Properties;
using Account.Services.Caching;
using Account.Services.Specifications;
using PetFamily.SharedKernel.Application.Exceptions;
using PetFamily.SharedKernel.Infrastructure.Abstractions;
using PetFamily.SharedKernel.Infrastructure.Caching;
using DomainAccount = Account.Domain.Aggregates.Account;

namespace Account.Services;

/// <inheritdoc/>
/// <param name="repository">Репозиторий над аккаунтами.</param>
/// <param name="cache">Сервис кеширования.</param>
internal sealed class AccountService(
    IRepository<DomainAccount> repository,
    ICacheService cache) : IAccountService
{
    /// <inheritdoc/>
    public async Task CreateAsync(Guid userId, CancellationToken ct)
    {
        var accountId = AccountId.Of(Guid.NewGuid());
        var account = DomainAccount.Create(accountId, userId);

        await repository.AddAsync(account, ct);
    }

    /// <inheritdoc/>
    public async Task UpdateProfileAsync(
        Guid userId,
        string? phone,
        int? experience,
        string? description,
        CancellationToken ct)
    {
        var account = await GetByUserIdAsync(userId, ct);

        var phoneNumber = phone != null ? PhoneNumber.Of(phone) : null;
        var ageExperience = experience.HasValue ? AgeExperience.Of(experience.Value) : null;
        var desc = description != null ? Description.Of(description) : null;

        account.UpdateProfile(phoneNumber, ageExperience, desc);

        await repository.UpdateAsync(account, ct);

        await cache.RemoveAsync(CacheKeys.AccountByUserId(userId), ct);
    }

    /// <inheritdoc/>
    public async Task UpdatePhotoAsync(Guid userId, Guid? photoId, CancellationToken ct)
    {
        var account = await GetByUserIdAsync(userId, ct);

        var photo = photoId.HasValue ? Photo.Create(photoId.Value) : null;

        account.UpdatePhoto(photo);

        await repository.UpdateAsync(account, ct);

        await cache.RemoveAsync(CacheKeys.AccountByUserId(userId), ct);
    }

    /// <inheritdoc/>
    public async Task<DomainAccount> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var specification = new GetByUserIdSpecification(userId);

        var account = await repository.FirstOrDefaultAsync(specification, ct);

        if (account == null)
        {
            throw new EntityNotFoundException<DomainAccount>(userId);
        }

        return account;
    }
}