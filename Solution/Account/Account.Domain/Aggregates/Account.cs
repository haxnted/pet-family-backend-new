using Account.Domain.Aggregates.ValueObjects;
using Account.Domain.Aggregates.ValueObjects.Identifiers;
using Account.Domain.Aggregates.ValueObjects.Properties;
using PetFamily.SharedKernel.Domain.Primitives;

namespace Account.Domain.Aggregates;

/// <summary>
/// Агрегат-сущность Аккаунт (профиль пользователя).
/// </summary>
public sealed class Account : Entity<AccountId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private Account(AccountId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="userId">Идентификатор пользователя из Auth.</param>
    private Account(AccountId id, Guid userId) : base(id)
    {
        UserId = userId;
    }

    /// <summary>
    /// Идентификатор пользователя из Auth модуля.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Фотография профиля (ссылка на FileStorage).
    /// </summary>
    public Photo? Photo { get; private set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public PhoneNumber? PhoneNumber { get; private set; }

    /// <summary>
    /// Опыт (в годах).
    /// </summary>
    public AgeExperience? AgeExperience { get; private set; }

    /// <summary>
    /// Описание профиля.
    /// </summary>
    public Description? Description { get; private set; }

    /// <summary>
    /// Фабричный метод для создания аккаунта <see cref="Account"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="userId">Идентификатор пользователя из Auth.</param>
    /// <returns>Аккаунт <see cref="Account"/>.</returns>
    public static Account Create(AccountId id, Guid userId)
    {
        return new Account(id, userId);
    }

    /// <summary>
    /// Обновить профильные данные.
    /// </summary>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="ageExperience">Опыт (в годах).</param>
    /// <param name="description">Описание.</param>
    public void UpdateProfile(
        PhoneNumber? phoneNumber,
        AgeExperience? ageExperience,
        Description? description)
    {
        PhoneNumber = phoneNumber;
        AgeExperience = ageExperience;
        Description = description;
    }

    /// <summary>
    /// Обновить фотографию профиля.
    /// </summary>
    /// <param name="photo">Фотография (null для удаления).</param>
    public void UpdatePhoto(Photo? photo)
    {
        Photo = photo;
    }
}