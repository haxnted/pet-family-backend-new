using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;
using VolunteerManagement.Domain.Aggregates.Shelters.Entities;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

namespace VolunteerManagement.Domain.Aggregates.Shelters;

/// <summary>
/// Агрегат-сущность Приют.
/// </summary>
public sealed class Shelter : SoftDeletableEntity<ShelterId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private Shelter(ShelterId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private Shelter(
        ShelterId id,
        ShelterName name,
        Address address,
        PhoneNumber phoneNumber,
        Description description,
        WorkingHours workingHours,
        int capacity) : base(id)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Description = description;
        WorkingHours = workingHours;
        Capacity = capacity;
        Status = ShelterStatus.Active;
    }

    /// <summary>
    /// Название приюта.
    /// </summary>
    public ShelterName Name { get; private set; } = null!;

    /// <summary>
    /// Адрес приюта.
    /// </summary>
    public Address Address { get; private set; } = null!;

    /// <summary>
    /// Контактный номер телефона.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; } = null!;

    /// <summary>
    /// Описание приюта.
    /// </summary>
    public Description Description { get; private set; } = null!;

    /// <summary>
    /// Режим работы.
    /// </summary>
    public WorkingHours WorkingHours { get; private set; } = null!;

    /// <summary>
    /// Максимальное количество питомцев.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Статус приюта.
    /// </summary>
    public ShelterStatus Status { get; private set; }

    private readonly List<VolunteerAssignment> _volunteerAssignments = [];

    /// <summary>
    /// Коллекция назначений волонтёров.
    /// </summary>
    public IReadOnlyList<VolunteerAssignment> VolunteerAssignments => _volunteerAssignments.AsReadOnly();

    /// <summary>
    /// Фабричный метод для создания приюта <see cref="Shelter"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Название.</param>
    /// <param name="address">Адрес.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="description">Описание.</param>
    /// <param name="workingHours">Режим работы.</param>
    /// <param name="capacity">Вместимость.</param>
    /// <exception cref="DomainException">
    /// Если вместимость меньше или равна нулю.
    /// </exception>
    public static Shelter Create(
        ShelterId id,
        ShelterName name,
        Address address,
        PhoneNumber phoneNumber,
        Description description,
        WorkingHours workingHours,
        int capacity)
    {
        if (capacity <= 0)
        {
            throw new DomainException("Вместимость приюта должна быть больше нуля.");
        }

        return new Shelter(id, name, address, phoneNumber, description, workingHours, capacity);
    }

    /// <summary>
    /// Обновить основную информацию о приюте.
    /// </summary>
    /// <param name="name">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="workingHours">Режим работы.</param>
    /// <param name="capacity">Вместимость.</param>
    public void UpdateMainInfo(
        ShelterName name,
        Description description,
        PhoneNumber phoneNumber,
        WorkingHours workingHours,
        int capacity)
    {
        if (capacity <= 0)
        {
            throw new DomainException("Вместимость приюта должна быть больше нуля.");
        }

        Name = name;
        Description = description;
        PhoneNumber = phoneNumber;
        WorkingHours = workingHours;
        Capacity = capacity;
    }

    /// <summary>
    /// Обновить адрес приюта.
    /// </summary>
    /// <param name="address">Новый адрес.</param>
    public void UpdateAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        Address = address;
    }

    /// <summary>
    /// Изменить статус приюта.
    /// </summary>
    /// <param name="newStatus">Новый статус.</param>
    public void ChangeStatus(ShelterStatus newStatus)
    {
        if (Status == newStatus)
        {
            throw new DomainException("Приют уже находится в данном статусе.");
        }

        Status = newStatus;
    }

    /// <summary>
    /// Назначить волонтёра в приют.
    /// </summary>
    /// <param name="assignment">Назначение.</param>
    /// <exception cref="DomainException">
    /// Если волонтёр уже активно назначен в этот приют.
    /// </exception>
    public void AssignVolunteer(VolunteerAssignment assignment)
    {
        ArgumentNullException.ThrowIfNull(assignment);

        var existing = _volunteerAssignments.FirstOrDefault(a => a.VolunteerId == assignment.VolunteerId && a.IsActive);

        if (existing != null)
        {
            throw new DomainException("Волонтёр уже назначен в этот приют.");
        }

        _volunteerAssignments.Add(assignment);
    }

    /// <summary>
    /// Убрать волонтёра из приюта (деактивировать назначение).
    /// </summary>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <exception cref="DomainException">
    /// Если активное назначение не найдено.
    /// </exception>
    public void RemoveVolunteer(Guid volunteerId)
    {
        var assignment = _volunteerAssignments.FirstOrDefault(a => a.VolunteerId == volunteerId && a.IsActive);

        if (assignment == null)
        {
            throw new DomainException("Активное назначение волонтёра не найдено.");
        }

        assignment.Deactivate();
    }

    /// <summary>
    /// Получить назначение волонтёра по идентификатору.
    /// </summary>
    /// <param name="assignmentId">Идентификатор назначения.</param>
    /// <exception cref="DomainException">
    /// Если назначение не найдено.
    /// </exception>
    public VolunteerAssignment GetAssignmentById(VolunteerAssignmentId assignmentId)
    {
        var assignment = _volunteerAssignments.FirstOrDefault(a => a.Id == assignmentId);

        if (assignment == null)
        {
            throw new DomainException("Назначение волонтёра не найдено.");
        }

        return assignment;
    }
}