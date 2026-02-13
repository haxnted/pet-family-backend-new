using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Domain.Aggregates.Volunteers.Enums;
using VolunteerManagement.Services.Volunteers.Dtos;
using VolunteerManagement.Services.Volunteers.Pets;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Pets;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class PetCommandHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task AddPet_ShouldPersistPetWithVolunteer()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        var species = SpeciesBuilder.Default().Build();
        var breed = VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities.Breed.Create(
            VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties.BreedName.Of("Лабрадор"),
            species.Id);
        species.AddBreed(breed);
        await InsertAsync(species);

        // Act
        var petId = Guid.Empty;
        await ExecuteInScopeAsync(async sp =>
        {
            var petService = sp.GetRequiredService<IPetService>();
            petId = await petService.AddPet(
                volunteer.Id.Value,
                "ТестовыйПитомец",
                "Достаточно длинное описание для теста питомца не менее десяти",
                "Информация о здоровье питомца для тестовых сценариев тоже",
                breed.Id.Value,
                species.Id.Value,
                25.0,
                50.0,
                DateTime.UtcNow.AddYears(-2),
                false,
                true,
                (int)HelpStatusPet.NeedsHelp,
                Enumerable.Empty<RequisiteDto>(),
                CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var savedVolunteer = await db.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            savedVolunteer.Should().NotBeNull();
            savedVolunteer!.Pets.Should().HaveCount(1);
            savedVolunteer.Pets.First().NickName.Value.Should().Be("ТестовыйПитомец");
        });
    }

    [Fact]
    public async Task DeletePet_ShouldRemoveFromDb()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            await service.DeletePet(volunteer.Id.Value, pet.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            found!.Pets.Should().BeEmpty();
        });
    }

    [Fact]
    public async Task SoftDeletePet_ShouldMarkAsDeleted()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            await service.SoftDeletePetAsync(volunteer.Id.Value, pet.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Pets
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == pet.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task RestorePet_ShouldUndelete()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet);
        volunteer.SoftRemovePet(pet);
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            await service.RestorePetAsync(volunteer.Id.Value, pet.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Pets
                .FirstOrDefaultAsync(p => p.Id == pet.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeFalse();
        });
    }

    [Fact]
    public async Task MovePet_ShouldChangePosition()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet1 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet2 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet3 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            await service.MovePetAsync(volunteer.Id.Value, pet3.Id.Value, 1, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            var movedPet = found!.Pets.First(p => p.Id == pet3.Id);
            movedPet.Position.Value.Should().Be(1);
        });
    }
}