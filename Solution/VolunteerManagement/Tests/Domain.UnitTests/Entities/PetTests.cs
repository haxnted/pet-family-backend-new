using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Entities;

public sealed class PetTests : UnitTestBase
{
    #region Create Tests

    [Fact]
    public void Create_ShouldSetDefaultValues()
    {
        var pet = PetBuilder.Default().Build();

        pet.Should().NotBeNull();
        pet.Position.Value.Should().Be(0);
        pet.Photos.Should().BeEmpty();
        pet.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        pet.BookerId.Should().BeNull();
        pet.ShelterId.Should().BeNull();
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_ShouldChangeFields()
    {
        var pet = PetBuilder.Default().Build();
        var newDescription = Description.Of("Обновленное описание питомца для тестирования");
        var newHealth = Description.Of("Обновленная информация о здоровье питомца");
        var newAttributes = PetPhysicalAttributes.Of(30.0, 60.0);
        var newRequisites = new List<Requisite>
        {
            Requisite.Of("Паспорт", "Ветеринарный паспорт международного образца")
        };

        pet.Update(newDescription, newHealth, newAttributes, true, true, HelpStatusPet.LookingForHome, newRequisites);

        pet.Description.Should().Be(newDescription);
        pet.HealthInformation.Should().Be(newHealth);
        pet.PhysicalAttributes.Should().Be(newAttributes);
        pet.IsCastrated.Should().BeTrue();
        pet.IsVaccinated.Should().BeTrue();
        pet.HelpStatus.Should().Be(HelpStatusPet.LookingForHome);
    }

    [Fact]
    public void Update_WhenDeleted_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();
        pet.Delete();

        var act = () => pet.Update(
            Description.Of("Обновленное описание питомца для тестирования"),
            Description.Of("Обновленная информация о здоровье питомца"),
            PetPhysicalAttributes.Of(10, 20),
            false, false, HelpStatusPet.NeedsHelp, []);

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region ChangePosition Tests

    [Fact]
    public void ChangePosition_ShouldChangePosition()
    {
        var pet = PetBuilder.Default().Build();

        pet.ChangePosition(Position.Of(5));

        pet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void ChangePosition_WhenSamePosition_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.ChangePosition(Position.Of(0));

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region Shelter Assignment Tests

    [Fact]
    public void AssignToShelter_ShouldSetShelterId()
    {
        var pet = PetBuilder.Default().Build();
        var shelterId = Guid.NewGuid();

        pet.AssignToShelter(shelterId);

        pet.ShelterId.Should().Be(shelterId);
    }

    [Fact]
    public void AssignToShelter_WithEmptyGuid_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.AssignToShelter(Guid.Empty);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void RemoveFromShelter_ShouldClearShelterId()
    {
        var pet = PetBuilder.Default().Build();
        pet.AssignToShelter(Guid.NewGuid());

        pet.RemoveFromShelter();

        pet.ShelterId.Should().BeNull();
    }

    #endregion

    #region Photo Tests

    [Fact]
    public void AddPhotos_ShouldAddPhotos()
    {
        var pet = PetBuilder.Default().Build();
        var photos = new List<Photo> { Photo.Create(Guid.NewGuid()), Photo.Create(Guid.NewGuid()) };

        pet.AddPhotos(photos);

        pet.Photos.Should().HaveCount(2);
    }

    [Fact]
    public void AddPhotos_WithEmptyList_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.AddPhotos([]);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddPhotos_WithNull_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.AddPhotos(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemovePhoto_ShouldRemovePhoto()
    {
        var pet = PetBuilder.Default().Build();
        var photoId = Guid.NewGuid();
        pet.AddPhotos([Photo.Create(photoId)]);

        pet.RemovePhoto(photoId);

        pet.Photos.Should().BeEmpty();
    }

    [Fact]
    public void RemovePhoto_WithNonExistent_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.RemovePhoto(Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdatePhotos_ShouldReplaceAll()
    {
        var pet = PetBuilder.Default().Build();
        pet.AddPhotos([Photo.Create(Guid.NewGuid())]);
        var newPhotos = new List<Photo> { Photo.Create(Guid.NewGuid()), Photo.Create(Guid.NewGuid()) };

        pet.UpdatePhotos(newPhotos);

        pet.Photos.Should().HaveCount(2);
    }

    #endregion

    #region Reservation/Adoption Tests

    [Fact]
    public void Reserve_ShouldSetBookedStatus()
    {
        var pet = PetBuilder.Default()
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        var bookerId = Guid.NewGuid();

        pet.Reserve(bookerId);

        pet.HelpStatus.Should().Be(HelpStatusPet.Booked);
        pet.BookerId.Should().Be(bookerId);
    }

    [Fact]
    public void Reserve_WhenAlreadyBooked_ShouldThrow()
    {
        var pet = PetBuilder.Default()
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        pet.Reserve(Guid.NewGuid());

        var act = () => pet.Reserve(Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Reserve_WhenFoundHome_ShouldThrow()
    {
        var pet = PetBuilder.Default()
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        pet.Reserve(Guid.NewGuid());
        pet.Adopt();

        var act = () => pet.Reserve(Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Reserve_WhenDeleted_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();
        pet.Delete();

        var act = () => pet.Reserve(Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void CancelReservation_ShouldResetStatus()
    {
        var pet = PetBuilder.Default()
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        pet.Reserve(Guid.NewGuid());

        pet.CancelReservation();

        pet.HelpStatus.Should().Be(HelpStatusPet.LookingForHome);
        pet.BookerId.Should().BeNull();
    }

    [Fact]
    public void CancelReservation_WhenNotBooked_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.CancelReservation();

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Adopt_ShouldSetFoundHomeStatus()
    {
        var pet = PetBuilder.Default()
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        pet.Reserve(Guid.NewGuid());

        pet.Adopt();

        pet.HelpStatus.Should().Be(HelpStatusPet.FoundHome);
    }

    [Fact]
    public void Adopt_WhenNotBooked_ShouldThrow()
    {
        var pet = PetBuilder.Default().Build();

        var act = () => pet.Adopt();

        act.Should().Throw<DomainException>();
    }

    #endregion
}
