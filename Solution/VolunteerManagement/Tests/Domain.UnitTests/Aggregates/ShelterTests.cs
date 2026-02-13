using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.Shelters.ValueObjects.Properties;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Aggregates;

public sealed class ShelterTests : UnitTestBase
{
    #region Create Tests

    [Fact]
    public void Create_WithValidParams_ShouldCreateShelter()
    {
        var shelter = ShelterBuilder.Default().Build();

        shelter.Should().NotBeNull();
        shelter.Status.Should().Be(ShelterStatus.Active);
        shelter.VolunteerAssignments.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithZeroCapacity_ShouldThrow()
    {
        var act = () => ShelterBuilder.Default().WithCapacity(0).Build();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WithNegativeCapacity_ShouldThrow()
    {
        var act = () => ShelterBuilder.Default().WithCapacity(-1).Build();

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region UpdateMainInfo Tests

    [Fact]
    public void UpdateMainInfo_ShouldUpdateFields()
    {
        var shelter = ShelterBuilder.Default().Build();
        var newName = ShelterName.Of("Новый приют");
        var newDescription = Description.Of("Обновленное описание для тестового приюта");
        var newPhone = PhoneNumber.Of("79998887766");
        var newHours = WorkingHours.Of(new TimeOnly(8, 0), new TimeOnly(20, 0));

        shelter.UpdateMainInfo(newName, newDescription, newPhone, newHours, 100);

        shelter.Name.Should().Be(newName);
        shelter.Description.Should().Be(newDescription);
        shelter.PhoneNumber.Should().Be(newPhone);
        shelter.WorkingHours.Should().Be(newHours);
        shelter.Capacity.Should().Be(100);
    }

    [Fact]
    public void UpdateMainInfo_WithZeroCapacity_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.UpdateMainInfo(
            ShelterName.Of("Тест"),
            Description.Of("Обновленное описание для тестового приюта"),
            PhoneNumber.Of("71234567890"),
            WorkingHours.Of(new TimeOnly(9, 0), new TimeOnly(18, 0)),
            0);

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region UpdateAddress Tests

    [Fact]
    public void UpdateAddress_ShouldChangeAddress()
    {
        var shelter = ShelterBuilder.Default().Build();
        var newAddress = Address.Of("ул. Новая 5", "Питер", "Ленинградская область", "654321");

        shelter.UpdateAddress(newAddress);

        shelter.Address.Should().Be(newAddress);
    }

    [Fact]
    public void UpdateAddress_WithNull_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.UpdateAddress(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region ChangeStatus Tests

    [Fact]
    public void ChangeStatus_ShouldUpdateStatus()
    {
        var shelter = ShelterBuilder.Default().Build();

        shelter.ChangeStatus(ShelterStatus.TemporaryClosed);

        shelter.Status.Should().Be(ShelterStatus.TemporaryClosed);
    }

    [Fact]
    public void ChangeStatus_WithSameStatus_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.ChangeStatus(ShelterStatus.Active);

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region AssignVolunteer Tests

    [Fact]
    public void AssignVolunteer_ShouldAddToCollection()
    {
        var shelter = ShelterBuilder.Default().Build();
        var assignment = VolunteerAssignmentBuilder.Default().Build();

        shelter.AssignVolunteer(assignment);

        shelter.VolunteerAssignments.Should().HaveCount(1);
        shelter.VolunteerAssignments.Should().Contain(assignment);
    }

    [Fact]
    public void AssignVolunteer_WhenAlreadyActive_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();
        var volunteerId = Guid.NewGuid();
        var assignment1 = VolunteerAssignmentBuilder.Default()
            .WithVolunteerId(volunteerId)
            .Build();
        var assignment2 = VolunteerAssignmentBuilder.Default()
            .WithVolunteerId(volunteerId)
            .Build();

        shelter.AssignVolunteer(assignment1);
        var act = () => shelter.AssignVolunteer(assignment2);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AssignVolunteer_WithNull_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.AssignVolunteer(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region RemoveVolunteer Tests

    [Fact]
    public void RemoveVolunteer_ShouldDeactivateAssignment()
    {
        var shelter = ShelterBuilder.Default().Build();
        var volunteerId = Guid.NewGuid();
        var assignment = VolunteerAssignmentBuilder.Default()
            .WithVolunteerId(volunteerId)
            .Build();
        shelter.AssignVolunteer(assignment);

        shelter.RemoveVolunteer(volunteerId);

        shelter.VolunteerAssignments.First().IsActive.Should().BeFalse();
    }

    [Fact]
    public void RemoveVolunteer_WhenNotFound_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.RemoveVolunteer(Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region GetAssignmentById Tests

    [Fact]
    public void GetAssignmentById_ShouldReturnAssignment()
    {
        var shelter = ShelterBuilder.Default().Build();
        var assignment = VolunteerAssignmentBuilder.Default().Build();
        shelter.AssignVolunteer(assignment);

        var result = shelter.GetAssignmentById(assignment.Id);

        result.Should().Be(assignment);
    }

    [Fact]
    public void GetAssignmentById_WithNonExistent_ShouldThrow()
    {
        var shelter = ShelterBuilder.Default().Build();

        var act = () => shelter.GetAssignmentById(VolunteerAssignmentId.Of(Guid.NewGuid()));

        act.Should().Throw<DomainException>();
    }

    #endregion
}
