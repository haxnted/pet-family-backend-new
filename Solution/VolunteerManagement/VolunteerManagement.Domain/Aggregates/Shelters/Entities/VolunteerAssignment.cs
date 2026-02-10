using PetFamily.SharedKernel.Domain.Primitives;
using PetFamily.SharedKernel.Domain.Exceptions;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

namespace VolunteerManagement.Domain.Aggregates.Shelters.Entities;

/// <summary>
/// Сущность назначения волонтёра в приют.
/// </summary>
public sealed class VolunteerAssignment : Entity<VolunteerAssignmentId>
{
    /// <summary>
    /// EF Constructor.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private VolunteerAssignment(VolunteerAssignmentId id) : base(id)
    {
    }

    /// <summary>
    /// Приватный конструктор для фабричного метода.
    /// </summary>
    private VolunteerAssignment(
        VolunteerAssignmentId id,
        Guid volunteerId,
        VolunteerRole role,
        DateTime assignedAt) : base(id)
    {
        VolunteerId = volunteerId;
        Role = role;
        AssignedAt = assignedAt;
        IsActive = true;
    }

    /// <summary>
    /// Идентификатор волонтёра из агрегата Volunteer.
    /// </summary>
    public Guid VolunteerId { get; private set; }

    /// <summary>
    /// Роль волонтёра в приюте.
    /// </summary>
    public VolunteerRole Role { get; private set; }

    /// <summary>
    /// Дата назначения.
    /// </summary>
    public DateTime AssignedAt { get; private set; }

    /// <summary>
    /// Активно ли назначение.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Фабричный метод для создания назначения волонтёра <see cref="VolunteerAssignment"/>.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="volunteerId">Идентификатор волонтёра.</param>
    /// <param name="role">Роль в приюте.</param>
    public static VolunteerAssignment Create(
        VolunteerAssignmentId id,
        Guid volunteerId,
        VolunteerRole role)
    {
        if (volunteerId == Guid.Empty)
        {
            throw new DomainException("Идентификатор волонтёра не может быть пустым.");
        }

        return new VolunteerAssignment(id, volunteerId, role, DateTime.UtcNow);
    }

    /// <summary>
    /// Изменить роль волонтёра в приюте.
    /// </summary>
    /// <param name="newRole">Новая роль.</param>
    public void ChangeRole(VolunteerRole newRole)
    {
        if (Role == newRole)
        {
            throw new DomainException("Волонтёр уже имеет эту роль.");
        }

        Role = newRole;
    }

    /// <summary>
    /// Деактивировать назначение.
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Назначение уже неактивно.");
        }

        IsActive = false;
    }

    /// <summary>
    /// Активировать назначение.
    /// </summary>
    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("Назначение уже активно.");
        }

        IsActive = true;
    }
}
