using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Domain.Aggregates.Volunteers.Entities;
using PetFamily.SharedKernel.Domain.Exceptions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Domain.Aggregates.Volunteers;

/// <summary>
/// Агрегат-сущность Волонтер.
/// </summary>
public sealed class Volunteer : SoftDeletableEntity<VolunteerId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="fullName">Фамилия, Имя, Отчество.</param>
    private Volunteer(
        VolunteerId id,
        FullName fullName) : base(id)
    {
        FullName = fullName;
    }

    /// <summary>
    /// Фамилия, Имя, Отчество.
    /// </summary>
    public FullName FullName { get; private set; }

    /// <summary>
    /// Идентификатор пользователя из Auth модуля (если волонтер создан через регистрацию).
    /// </summary>
    public Guid? UserId { get; private set; }

    private readonly List<Pet> _pets = [];

    /// <summary>
    /// Коллекция животных.
    /// </summary>
    public IReadOnlyList<Pet> Pets => _pets.AsReadOnly();

    /// <summary>
    /// Лимит на количество животных которых может создать волонтёр.
    /// </summary>
    public const int LimitPets = 100;

    /// <summary>
    /// Фабричный метод для создания сущности <see cref="Volunteer"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="fullName">Фамилия, Имя, Отчество.</param>
    /// <returns>Волонтер <see cref="Volunteer"/>.</returns>
    public static Volunteer Create(
        VolunteerId id,
        FullName fullName)
    {
        return new Volunteer(id, fullName);
    }

    /// <summary>
    /// Добавить животное.
    /// </summary>
    /// <param name="pet">Животное.</param>
    public void AddPet(Pet pet)
    {
        if (Pets.Count > LimitPets)
        {
            throw new DomainException($"Вы привысили допустимый лимит {LimitPets} на создание животных.");
        }

        ArgumentNullException.ThrowIfNull(pet);

        var indexPosition = _pets.Count == 0 ? 1 : _pets.Count + 1;
        var position = Position.Of(indexPosition);
        pet.ChangePosition(position);

        _pets.Add(pet);
    }

    /// <summary>
    /// Удалить полностью животное из базы данных.
    /// </summary>
    /// <param name="pet">Животное.</param>
    /// <exception cref="DomainException">
    /// Если животное с таким идентификатором отсутствует.
    /// </exception>
    public void HardRemovePet(Pet pet)
    {
        var petToRemove = _pets.FirstOrDefault(p => p.Id == pet.Id)
                          ?? throw new DomainException("Не найдено животное");

        _pets.Remove(petToRemove);
    }

    /// <summary>
    /// Удалить всех животных из базы данных.
    /// </summary>
    public void HardRemoveAllPets() => _pets.Clear();

    /// <summary>
    /// Мягкое удаление питомца.
    /// </summary>
    /// <param name="pet">Питомец для удаления.</param>
    /// <exception cref="DomainException">
    /// Если питомец с таким идентификатором отсутствует.
    /// </exception>
    public void SoftRemovePet(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);

        var existing = _pets.FirstOrDefault(p => p.Id == pet.Id);
        if (existing == null)
        {
            throw new DomainException("Питомец не найден.");
        }

        existing.Delete();
    }

    /// <summary>
    /// Восстановить питомца.
    /// </summary>
    /// <param name="pet">Питомец для восстановления.</param>
    /// <exception cref="DomainException">
    /// Если питомец с таким идентификатором отсутствует.
    /// </exception>
    public void RestorePet(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);

        var existing = _pets.FirstOrDefault(p => p.Id == pet.Id);
        if (existing == null)
        {
            throw new DomainException("Питомец не найден.");
        }

        existing.Restore();
    }

    /// <summary>
    /// Получить животное
    /// </summary>
    /// <param name="petId">Идентификатор животного.</param>
    /// <exception cref="DomainException">
    /// Если животное с таким идентификатором отсутствует.
    /// </exception>
    public Pet GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(x => x.Id == petId);

        if (pet == null)
        {
            throw new DomainException("Животное с таким идентификатором не найдено.");
        }

        return pet;
    }

    /// <summary>
    /// Забронировать питомца для усыновления.
    /// </summary>
    /// <param name="petId">Идентификатор питомца.</param>
    /// <param name="bookerId">Идентификатор усыновителя.</param>
    public void ReservePet(PetId petId, Guid bookerId)
    {
        var pet = GetPetById(petId);
        pet.Reserve(bookerId);
    }

    /// <summary>
    /// Отменить бронирование питомца (компенсация саги).
    /// </summary>
    /// <param name="petId">Идентификатор питомца.</param>
    public void CancelPetReservation(PetId petId)
    {
        var pet = GetPetById(petId);
        pet.CancelReservation();
    }

    /// <summary>
    /// Подтвердить усыновление питомца (перевести в статус "нашёл дом").
    /// </summary>
    /// <param name="petId">Идентификатор питомца.</param>
    public void AdoptPet(PetId petId)
    {
        var pet = GetPetById(petId);
        pet.Adopt();
    }

    /// <summary>
    /// Установить связь с пользователем из Auth модуля.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }

    /// <summary>
    /// Передвинуть животное на другую позицию.
    /// </summary>
    /// <param name="id">Идентификатор животного.</param>
    /// <param name="newIdx">Новый номер позиции.</param>
    /// <exception cref="DomainException">
    /// Если недостаточно животных для перемещения.
    /// Если позиция животного превышает размер списка животных.
    /// Если не удалось найти животное с таким идентификатором.
    /// Если животное уже находится на этой позиции.
    /// </exception>
    public void MovePet(PetId id, Position newIdx)
    {
        if (_pets.Count == 1)
            throw new DomainException("Недостаточно животных для перемещения.");

        if (newIdx.Value > Pets.Count)
            throw new DomainException("Позиция животного превышает размер списка животных");

        var pet = _pets.FirstOrDefault(p => p.Id == id);
        if (pet == null)
            throw new DomainException("Не удалось найти животное с таким идентификатором.");

        if (pet.Position == newIdx)
            throw new DomainException("Животное уже находится на этой позиции");

        var oldIdx = Position.Of(pet.Position.Value);
        UpdatePositions(newIdx, oldIdx);
        pet.ChangePosition(newIdx);
    }

    /// <summary>
    /// Поменять позициями животных.
    /// </summary>
    /// <param name="newIdx">Новый номер позиции.</param>
    /// <param name="oldIdx">Старый номер позиции.</param>
    private void UpdatePositions(Position newIdx, Position oldIdx)
    {
        var collection = newIdx.Value < oldIdx.Value
            ? _pets.Where(p => p.Position.Value >= newIdx.Value && p.Position.Value < oldIdx.Value)
            : _pets.Where(p => p.Position.Value > oldIdx.Value && p.Position.Value <= newIdx.Value);

        foreach (var entity in collection)
        {
            var condition = newIdx.Value < oldIdx.Value
                ? entity.Position.Value + 1
                : entity.Position.Value - 1;

            var position = Position.Of(condition);

            entity.ChangePosition(position);
        }
    }

    /// <inheritdoc/>
    public override void Restore()
    {
        base.Restore();

        foreach (var pet in _pets)
            pet.Restore();
    }

    /// <inheritdoc/>
    public override void Delete()
    {
        base.Delete();

        foreach (var pet in _pets)
            pet.Delete();
    }
}