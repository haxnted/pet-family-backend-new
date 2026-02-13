using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Services.Volunteers;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Volunteers;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class VolunteerQueryHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task GetVolunteerById_WithExisting_ShouldReturnVolunteer()
    {
        // Arrange
        var volunteer = VolunteerBuilder.Default()
            .WithFullName("Тест", "Тестов")
            .Build();
        await InsertAsync(volunteer);

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            var result = await service.GetAsync(volunteer.Id.Value, CancellationToken.None);

            result.Should().NotBeNull();
            result.FullName.Name.Should().Be("Тест");
            result.FullName.Surname.Should().Be("Тестов");
        });
    }

    [Fact]
    public async Task GetVolunteersWithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        for (var i = 0; i < 5; i++)
        {
            await InsertAsync(VolunteerBuilder.Default().Build());
        }

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            var result = await service.GetWithPaginationAsync(1, 3, CancellationToken.None);

            result.Should().HaveCount(3);
        });
    }

    [Fact]
    public async Task GetVolunteersWithPagination_SecondPage_ShouldReturnRemaining()
    {
        // Arrange
        for (var i = 0; i < 5; i++)
        {
            await InsertAsync(VolunteerBuilder.Default().Build());
        }

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IVolunteerService>();
            var result = await service.GetWithPaginationAsync(2, 3, CancellationToken.None);

            result.Should().HaveCount(2);
        });
    }
}
