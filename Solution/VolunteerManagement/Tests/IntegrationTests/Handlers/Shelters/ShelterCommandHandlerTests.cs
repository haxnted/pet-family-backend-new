using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Domain.Aggregates.Shelters.Enums;
using VolunteerManagement.Services.Shelters;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.Shelters;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class ShelterCommandHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task AddShelter_ShouldPersist()
    {
        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.AddAsync(
                "Тестовый приют",
                "ул. Тестовая 1", "Москва", "Московская область", "123456",
                "71234567890",
                "Описание тестового приюта для животных в городе Москва",
                new TimeOnly(9, 0), new TimeOnly(18, 0),
                50,
                CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var shelter = await db.Shelters.FirstOrDefaultAsync();
            shelter.Should().NotBeNull();
            shelter!.Name.Value.Should().Be("Тестовый приют");
            shelter.Status.Should().Be(ShelterStatus.Active);
            shelter.Capacity.Should().Be(50);
        });
    }

    [Fact]
    public async Task UpdateShelter_ShouldChangeFields()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.UpdateAsync(
                shelter.Id.Value,
                "Обновленный приют",
                "79998887766",
                "Обновленное описание тестового приюта для животных",
                new TimeOnly(8, 0), new TimeOnly(20, 0),
                100,
                CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters.FirstOrDefaultAsync(s => s.Id == shelter.Id);
            found!.Name.Value.Should().Be("Обновленный приют");
            found.Capacity.Should().Be(100);
        });
    }

    [Fact]
    public async Task UpdateShelterAddress_ShouldChangeAddress()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.UpdateAddressAsync(
                shelter.Id.Value,
                "ул. Новая 5", "Санкт-Петербург", "Ленинградская область", "654321",
                CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters.FirstOrDefaultAsync(s => s.Id == shelter.Id);
            found!.Address.City.Should().Be("Санкт-Петербург");
            found.Address.Street.Should().Be("ул. Новая 5");
        });
    }

    [Fact]
    public async Task SoftRemoveShelter_ShouldMarkAsDeleted()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.SoftRemoveAsync(shelter.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.Id == shelter.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task HardRemoveShelter_ShouldDeleteFromDb()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.HardRemoveAsync(shelter.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(s => s.Id == shelter.Id);

            found.Should().BeNull();
        });
    }

    [Fact]
    public async Task ChangeShelterStatus_ShouldUpdate()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.ChangeStatusAsync(
                shelter.Id.Value, (int)ShelterStatus.TemporaryClosed, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters.FirstOrDefaultAsync(s => s.Id == shelter.Id);
            found!.Status.Should().Be(ShelterStatus.TemporaryClosed);
        });
    }

    [Fact]
    public async Task AssignVolunteer_ShouldAddAssignment()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);
        var volunteerId = Guid.NewGuid();

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.AssignVolunteerAsync(
                shelter.Id.Value, volunteerId, (int)VolunteerRole.Caretaker, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters
                .Include(s => s.VolunteerAssignments)
                .FirstOrDefaultAsync(s => s.Id == shelter.Id);

            found!.VolunteerAssignments.Should().HaveCount(1);
            found.VolunteerAssignments.First().VolunteerId.Should().Be(volunteerId);
            found.VolunteerAssignments.First().IsActive.Should().BeTrue();
        });
    }

    [Fact]
    public async Task RemoveVolunteer_ShouldDeactivateAssignment()
    {
        // Arrange
        var shelter = ShelterBuilder.Default().Build();
        await InsertAsync(shelter);
        var volunteerId = Guid.NewGuid();

        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.AssignVolunteerAsync(
                shelter.Id.Value, volunteerId, (int)VolunteerRole.Caretaker, CancellationToken.None);
        });

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<IShelterService>();
            await service.RemoveVolunteerAsync(shelter.Id.Value, volunteerId, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Shelters
                .Include(s => s.VolunteerAssignments)
                .FirstOrDefaultAsync(s => s.Id == shelter.Id);

            found!.VolunteerAssignments.First().IsActive.Should().BeFalse();
        });
    }
}
