using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Shelters.Entities;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;

namespace VolunteerManagement.Tests.Domain.Builders;

/// <summary>
/// Builder для создания тестовых экземпляров VolunteerAssignment.
/// </summary>
public sealed class VolunteerAssignmentBuilder : IBuilder<VolunteerAssignment>
{
    private VolunteerAssignmentId _id = VolunteerAssignmentId.Of(Guid.NewGuid());
    private Guid _volunteerId = Guid.NewGuid();
    private VolunteerRole _role = VolunteerRole.Caretaker;

    public VolunteerAssignmentBuilder WithId(Guid id)
    {
        _id = VolunteerAssignmentId.Of(id);
        return this;
    }

    public VolunteerAssignmentBuilder WithVolunteerId(Guid volunteerId)
    {
        _volunteerId = volunteerId;
        return this;
    }

    public VolunteerAssignmentBuilder WithRole(VolunteerRole role)
    {
        _role = role;
        return this;
    }

    public VolunteerAssignment Build()
    {
        return VolunteerAssignment.Create(_id, _volunteerId, _role);
    }

    public static VolunteerAssignmentBuilder Default() => new();

    public static IReadOnlyList<VolunteerAssignment> BuildMany(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Default().Build())
            .ToList();
    }
}
