using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Shelters;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class ShelterQueryHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task GetShelterById_ShouldReturnShelter()
    {
        // Arrange
        var shelter = ShelterBuilder.Default()
            .WithName("Тестовый приют")
            .Build();
        await InsertAsync(shelter);

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            var result = await service.GetAsync(shelter.Id.Value, CancellationToken.None);

            result.Should().NotBeNull();
            result.Name.Value.Should().Be("Тестовый приют");
        });
    }

    [Fact]
    public async Task GetSheltersWithPagination_ShouldReturnPage()
    {
        // Arrange
        for (var i = 0; i < 5; i++)
        {
            await InsertAsync(ShelterBuilder.Default().Build());
        }

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            var result = await service.GetWithPaginationAsync(1, 3, CancellationToken.None);

            result.Should().HaveCount(3);
        });
    }

    [Fact]
    public async Task GetAssignment_ShouldReturnAssignment()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);
        var volunteerId = Guid.NewGuid();

        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.AssignVolunteerAsync(
                shelter.Id.Value, volunteerId, (int)VolunteerRole.Manager, CancellationToken.None);
        });

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            var shelterResult = await service.GetAsync(shelter.Id.Value, CancellationToken.None);
            shelterResult.VolunteerAssignments.Should().HaveCount(1);
            shelterResult.VolunteerAssignments.First().Role.Should().Be(VolunteerRole.Manager);
        });
    }
}
