using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Aggregates;

public sealed class VolunteerAdoptionTests : UnitTestBase
{
    #region ReservePet Tests

    [Fact]
    public void ReservePet_ShouldChangeStatusToBooked()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        volunteer.AddPet(pet);
        var adopterId = Guid.NewGuid();

        volunteer.ReservePet(pet.Id, adopterId);

        var reservedPet = volunteer.GetPetById(pet.Id);
        reservedPet.HelpStatus.Should().Be(HelpStatusPet.Booked);
        reservedPet.BookerId.Should().Be(adopterId);
    }

    [Fact]
    public void ReservePet_WhenPetNotFound_ShouldThrow()
    {
        var volunteer = VolunteerBuilder.Default().Build();

        var act = () => volunteer.ReservePet(PetId.Of(Guid.NewGuid()), Guid.NewGuid());

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region CancelPetReservation Tests

    [Fact]
    public void CancelPetReservation_ShouldResetToLookingForHome()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        volunteer.AddPet(pet);
        volunteer.ReservePet(pet.Id, Guid.NewGuid());

        volunteer.CancelPetReservation(pet.Id);

        var cancelledPet = volunteer.GetPetById(pet.Id);
        cancelledPet.HelpStatus.Should().Be(HelpStatusPet.LookingForHome);
        cancelledPet.BookerId.Should().BeNull();
    }

    #endregion

    #region AdoptPet Tests

    [Fact]
    public void AdoptPet_ShouldSetFoundHomeStatus()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        volunteer.AddPet(pet);
        volunteer.ReservePet(pet.Id, Guid.NewGuid());

        volunteer.AdoptPet(pet.Id);

        var adoptedPet = volunteer.GetPetById(pet.Id);
        adoptedPet.HelpStatus.Should().Be(HelpStatusPet.FoundHome);
    }

    [Fact]
    public void AdoptPet_WhenNotBooked_ShouldThrow()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        volunteer.AddPet(pet);

        var act = () => volunteer.AdoptPet(pet.Id);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void FullAdoptionFlow_ReserveThenCancelThenReReserve_ShouldWork()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithHelpStatus(HelpStatusPet.LookingForHome)
            .Build();
        volunteer.AddPet(pet);
        var adopter1 = Guid.NewGuid();
        var adopter2 = Guid.NewGuid();

        // First reserve and cancel
        volunteer.ReservePet(pet.Id, adopter1);
        volunteer.CancelPetReservation(pet.Id);

        // Re-reserve by different adopter
        volunteer.ReservePet(pet.Id, adopter2);

        var rePet = volunteer.GetPetById(pet.Id);
        rePet.HelpStatus.Should().Be(HelpStatusPet.Booked);
        rePet.BookerId.Should().Be(adopter2);
    }

    #endregion
}
