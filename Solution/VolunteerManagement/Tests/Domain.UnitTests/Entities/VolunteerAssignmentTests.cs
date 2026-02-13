using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Entities;

public sealed class VolunteerAssignmentTests : UnitTestBase
{
    #region Create Tests

    [Fact]
    public void Create_WithValidParams_ShouldCreate()
    {
        var assignment = VolunteerAssignmentBuilder.Default().Build();

        assignment.Should().NotBeNull();
        assignment.IsActive.Should().BeTrue();
        assignment.Role.Should().Be(VolunteerRole.Caretaker);
        assignment.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WithEmptyVolunteerId_ShouldThrow()
    {
        var act = () => VolunteerAssignmentBuilder.Default()
            .WithVolunteerId(Guid.Empty)
            .Build();

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region ChangeRole Tests

    [Fact]
    public void ChangeRole_ShouldChangeRole()
    {
        var assignment = VolunteerAssignmentBuilder.Default()
            .WithRole(VolunteerRole.Caretaker)
            .Build();

        assignment.ChangeRole(VolunteerRole.Manager);

        assignment.Role.Should().Be(VolunteerRole.Manager);
    }

    [Fact]
    public void ChangeRole_WithSameRole_ShouldThrow()
    {
        var assignment = VolunteerAssignmentBuilder.Default()
            .WithRole(VolunteerRole.Caretaker)
            .Build();

        var act = () => assignment.ChangeRole(VolunteerRole.Caretaker);

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region Deactivate/Activate Tests

    [Fact]
    public void Deactivate_ShouldSetInactive()
    {
        var assignment = VolunteerAssignmentBuilder.Default().Build();

        assignment.Deactivate();

        assignment.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrow()
    {
        var assignment = VolunteerAssignmentBuilder.Default().Build();
        assignment.Deactivate();

        var act = () => assignment.Deactivate();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Activate_WhenAlreadyActive_ShouldThrow()
    {
        var assignment = VolunteerAssignmentBuilder.Default().Build();

        var act = () => assignment.Activate();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Activate_AfterDeactivation_ShouldReactivate()
    {
        var assignment = VolunteerAssignmentBuilder.Default().Build();
        assignment.Deactivate();

        assignment.Activate();

        assignment.IsActive.Should().BeTrue();
    }

    #endregion
}
