using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Services.Volunteers.Pets;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Pets;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class PetQueryHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task GetPetById_ShouldReturnPet()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithNickName("ТестовыйПитомец")
            .Build();
        volunteer.AddPet(pet);
        await InsertAsync(volunteer);

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            var result = await service.GetPetById(
                volunteer.Id.Value, pet.Id.Value, CancellationToken.None);

            result.Should().NotBeNull();
            result.NickName.Value.Should().Be("ТестовыйПитомец");
        });
    }

    [Fact]
    public async Task GetPetsByVolunteerId_ShouldReturnAllPets()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default().Build();
        for (var i = 0; i < 3; i++)
        {
            volunteer.AddPet(PetBuilder.Default().WithVolunteerId(volunteer.Id).Build());
        }
        await InsertAsync(volunteer);

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IPetService>();
            var result = await service.GetPetsByVolunteerId(
                volunteer.Id.Value, CancellationToken.None);

            result.Should().HaveCount(3);
        });
    }
}
