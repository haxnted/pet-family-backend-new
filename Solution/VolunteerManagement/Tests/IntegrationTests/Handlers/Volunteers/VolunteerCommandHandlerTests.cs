using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Volunteers;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class VolunteerCommandHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task AddVolunteer_ShouldPersistVolunteer()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            await service.AddAsync("Иван", "Иванов", "Иванович", userId, CancellationToken.None);
        });

        // Assert (new scope)
        await ExecuteWithDbContextAsync(async db =>
        {
            var volunteer = await db.Volunteers
                .FirstOrDefaultAsync(v => v.UserId == userId);

            volunteer.Should().NotBeNull();
            volunteer!.FullName.Name.Should().Be("Иван");
            volunteer.FullName.Surname.Should().Be("Иванов");
        });
    }

    [Fact]
    public async Task SoftRemoveVolunteer_ShouldMarkAsDeleted()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            await service.SoftRemoveAsync(volunteer.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task HardRemoveVolunteer_ShouldDeleteFromDb()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            await service.HardRemoveAsync(volunteer.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            found.Should().BeNull();
        });
    }

    [Fact]
    public async Task ActivateAccount_ShouldRestoreVolunteer()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        volunteer.Delete();
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            await service.ActivateAccountVolunteerRequest(volunteer.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeFalse();
        });
    }

    [Fact]
    public async Task HardRemoveAllPets_ShouldClearPets()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet1 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        var pet2 = PetBuilder.Default().WithVolunteerId(volunteer.Id).Build();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        await InsertAsync(volunteer);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            await service.HardRemoveAllPetsAsync(volunteer.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            found.Should().NotBeNull();
            found!.Pets.Should().BeEmpty();
        });
    }
}
