using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using PetFamily.SharedKernel.Domain.Exceptions;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Aggregates;

public sealed class VolunteerTests : UnitTestBase
{
    #region AddPet Tests

    [Fact]
    public void AddPet_ShouldAddPetToCollection()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .Build();

        volunteer.AddPet(pet);

        volunteer.Pets.Should().HaveCount(1);
        volunteer.Pets.Should().Contain(pet);
    }

    [Fact]
    public void AddPet_ShouldAssignCorrectPosition()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet1 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet2 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        volunteer.Pets[0].Position.Value.Should().Be(1);
        volunteer.Pets[1].Position.Value.Should().Be(2);
    }

    [Fact]
    public void AddPet_WithNullPet_ShouldThrowArgumentNullException()
    {
        var volunteer = VolunteerBuilder.Default().Build();

        var act = () => volunteer.AddPet(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region RemovePet Tests

    [Fact]
    public void HardRemovePet_ShouldRemovePetFromCollection()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);

        volunteer.HardRemovePet(pet);

        volunteer.Pets.Should().BeEmpty();
    }

    [Fact]
    public void HardRemovePet_WithNonExistentPet_ShouldThrowException()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var nonExistentPet = PetBuilder.Default().Build();

        var act = () => volunteer.HardRemovePet(nonExistentPet);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void SoftRemovePet_ShouldMarkPetAsDeleted()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);

        volunteer.SoftRemovePet(pet);

        volunteer.Pets.Should().HaveCount(1);
        volunteer.Pets[0].IsSoftDeleted.Should().BeTrue();
    }

    [Fact]
    public void HardRemoveAllPets_ShouldClearPetsCollection()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        for (var i = 0; i < 5; i++)
        {
            volunteer.AddPet(PetBuilder.Default().WithVolunteerId(volunteer.Id).Build());
        }

        volunteer.HardRemoveAllPets();

        volunteer.Pets.Should().BeEmpty();
    }

    #endregion

    #region GetPetById Tests

    [Fact]
    public void GetPetById_WithExistingPet_ShouldReturnPet()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);

        var result = volunteer.GetPetById(pet.Id);

        result.Should().Be(pet);
    }

    [Fact]
    public void GetPetById_WithNonExistentPet_ShouldThrowException()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var nonExistentPetId = PetId.Of(Guid.NewGuid());

        var act = () => volunteer.GetPetById(nonExistentPetId);

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region MovePet Tests

    [Fact]
    public void MovePet_ShouldChangePosition()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet1 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet2 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet3 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();

        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        volunteer.MovePet(pet3.Id, Position.Of(1));

        pet3.Position.Value.Should().Be(1);
    }

    [Fact]
    public void MovePet_WithSinglePet_ShouldThrowException()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);

        var act = () => volunteer.MovePet(pet.Id, Position.Of(1));

        act.Should().Throw<DomainException>()
            .WithMessage("*Недостаточно*");
    }

    #endregion

    #region SoftDelete Tests

    [Fact]
    public void Delete_ShouldMarkVolunteerAndAllPetsAsDeleted()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet1 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet2 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        volunteer.Delete();

        volunteer.IsDeleted.Should().BeTrue();
        volunteer.Pets.Should().AllSatisfy(p => p.IsDeleted.Should().BeTrue());
    }

    [Fact]
    public void Restore_ShouldRestoreVolunteerAndAllPets()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);
        volunteer.Delete();

        volunteer.Restore();

        volunteer.IsDeleted.Should().BeFalse();
        volunteer.Pets.Should().AllSatisfy(p => p.IsDeleted.Should().BeFalse());
    }

    #endregion

    #region SetUserId Tests

    [Fact]
    public void SetUserId_ShouldSetUserId()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var userId = Guid.NewGuid();

        volunteer.SetUserId(userId);

        volunteer.UserId.Should().Be(userId);
    }

    #endregion
}