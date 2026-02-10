using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using PetFamily.SharedKernel.Domain.Exceptions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Domain.Aggregates.Volunteers.Entities;

/// <summary>
/// Класс-сущность Животное.
/// </summary>
public sealed class Pet : SoftDeletableEntity<PetId>
{
    /// <summary>
    /// EF Constructor.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    protected Pet(PetId id) : base(id)
    {
    }

    /// <summary>
    /// Идентификатор волонтера.
    /// </summary>
    public VolunteerId VolunteerId { get; }

    /// <summary>
    /// Кличка.
    /// </summary>
    public NickName NickName { get; private set; } = null!;

    /// <summary>
    /// Общая информация.
    /// </summary>
    public Description Description { get; private set; } = null!;

    /// <summary>
    /// Информация о здоровье.
    /// </summary>
    public Description HealthInformation { get; private set; } = null!;

    /// <summary>
    /// Порода.
    /// </summary>
    public Guid BreedId { get; private set; }

    /// <summary>
    /// Вид.
    /// </summary>
    public Guid SpeciesId { get; private set; }

    /// <summary>
    /// Физические характеристики.
    /// </summary>
    public PetPhysicalAttributes PhysicalAttributes { get; private set; } = null!;

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// Флаг кастрации.
    /// </summary>
    public bool IsCastrated { get; private set; }

    /// <summary>
    /// Флаг вакцинации.
    /// </summary>
    public bool IsVaccinated { get; private set; }

    /// <summary>
    /// Текущий статус.
    /// </summary>
    public HelpStatusPet HelpStatus { get; private set; }

    /// <summary>
    /// Идентификатор приюта.
    /// </summary>
    public Guid? ShelterId { get; private set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime DateCreated { get; private set; }

    /// <summary>
    /// Позиция животного в коллекции животных.
    /// </summary>
    public Position Position { get; private set; }

    private readonly List<Photo> _photos = [];

    /// <summary>
    /// Коллекция фотографий.
    /// </summary>
    public IReadOnlyCollection<Photo> Photos { get; private set; }

    /// <summary>
    /// Коллекция реквизитов.
    /// </summary>
    public IReadOnlyList<Requisite> RequisiteList { get; private set; }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="volunteerId">Идентификатор Волонтёра.</param>
    /// <param name="nickName">Кличка.</param>
    /// <param name="description">Общее описание.</param>
    /// <param name="healthInformation">Информация о здоровье.</param>
    /// <param name="attributes">Физические характеристики.</param>
    /// <param name="speciesId">Порода.</param>
    /// <param name="breedId">Вид.</param>
    /// <param name="birthDate">Дата рождения.</param>
    /// <param name="isCastrated">Флаг кастрации.</param>
    /// <param name="isVaccinated">Флаг вакцинации.</param>
    /// <param name="helpStatus">Текущий статус.</param>
    /// <param name="dateCreated">Дата создания.</param>
    /// <param name="petPhotoList">Коллекция фотографий.</param>
    /// <param name="requisiteList">Коллекция реквизитов.</param>
    /// <param name="position">Позиция.</param>
    private Pet(
        PetId id,
        VolunteerId volunteerId,
        NickName nickName,
        Description description,
        Description healthInformation,
        PetPhysicalAttributes attributes,
        Guid speciesId,
        Guid breedId,
        DateTime birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        DateTime dateCreated,
        List<Photo> petPhotoList,
        List<Requisite> requisiteList,
        Position position) : base(id)
    {
        Photos = petPhotoList.AsReadOnly();
        RequisiteList = requisiteList.AsReadOnly();
        VolunteerId = volunteerId;
        NickName = nickName;
        Description = description;
        HealthInformation = healthInformation;
        PhysicalAttributes = attributes;
        BreedId = breedId;
        SpeciesId = speciesId;
        BirthDate = birthDate;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        DateCreated = dateCreated;
        Position = position;
    }

    /// <summary>
    /// Фабричный метод для создания сущности <see cref="Pet"/>.
    /// </summary>
    public static Pet Create(
        PetId id,
        VolunteerId volunteerId,
        NickName nickName,
        Description generalDescription,
        Description healthInformation,
        PetPhysicalAttributes attributes,
        Guid speciesId,
        Guid breedId,
        DateTime birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        List<Requisite> requisiteList)
    {
        return new Pet(
            id,
            volunteerId,
            nickName,
            generalDescription,
            healthInformation,
            attributes,
            speciesId,
            breedId,
            birthDate,
            isCastrated,
            isVaccinated,
            helpStatus,
            DateTime.UtcNow,
            [],
            requisiteList,
            Position.Of(0));
    }

    /// <summary>
    /// Обновить информацию о животном.
    /// </summary>
    public void Update(
        Description generalDescription,
        Description healthInformation,
        PetPhysicalAttributes attributes,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatus,
        IReadOnlyList<Requisite> requisiteList
    )
    {
        if (IsDeleted)
        {
            throw new DomainException("Нельзя обновить животное которое удалено.");
        }

        Description = generalDescription;
        HealthInformation = healthInformation;
        PhysicalAttributes = attributes;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatus;
        RequisiteList = requisiteList;
    }

    /// <summary>
    /// Сменить позицию.
    /// </summary>
    /// <param name="newPosition">Позиция.</param>
    public void ChangePosition(Position newPosition)
    {
        if (Position == newPosition)
        {
            throw new DomainException("Животное уже стоит на этой позиции.");
        }

        Position = newPosition;
    }

    /// <summary>
    /// Привязать питомца к приюту.
    /// </summary>
    /// <param name="shelterId">Идентификатор приюта.</param>
    public void AssignToShelter(Guid shelterId)
    {
        if (shelterId == Guid.Empty)
            throw new DomainException("Идентификатор приюта не может быть пустым.");

        ShelterId = shelterId;
    }

    /// <summary>
    /// Отвязать питомца от приюта.
    /// </summary>
    public void RemoveFromShelter()
    {
        ShelterId = null;
    }

    /// <summary>
    /// Проверить, удалено ли животное.
    /// </summary>
    public bool IsSoftDeleted => IsDeleted;

    /// <summary>
    /// Добавить фотографии к питомцу.
    /// </summary>
    /// <param name="photos">Список фотографий.</param>
    public void AddPhotos(List<Photo> photos)
    {
        ArgumentNullException.ThrowIfNull(photos);

        if (photos.Count == 0)
            throw new DomainException("Список фотографий пуст.");

        _photos.AddRange(photos);
    }

    /// <summary>
    /// Удалить фотографию.
    /// </summary>
    /// <param name="photoId">Идентификатор фотографии.</param>
    public void RemovePhoto(Guid photoId)
    {
        var photoToRemove = _photos.FirstOrDefault(p => p.Value == photoId);

        if (photoToRemove == null)
            throw new DomainException("Фотография не найдена.");

        _photos.Remove(photoToRemove);
    }

    /// <summary>
    /// Обновить фотографии питомца (заменить все).
    /// </summary>
    /// <param name="photos">Новый список фотографий.</param>
    public void UpdatePhotos(List<Photo> photos)
    {
        ArgumentNullException.ThrowIfNull(photos);

        _photos.Clear();
        _photos.AddRange(photos);
    }
}