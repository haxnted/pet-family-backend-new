using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Identifiers;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Repository;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class VolunteerRepositoryTests : VolunteerManagementIntegrationTestBase
{
    public VolunteerRepositoryTests(VolunteerManagementWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task AddAsync_ShouldPersistVolunteer()
    {
        var volunteer = VolunteerBuilder.Default()
            .WithFullName("Иван", "Иванов", "Иванович")
            .Build();

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            dbContext.Volunteers.Add(volunteer);
        });

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var savedVolunteer = await dbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            savedVolunteer.Should().NotBeNull();
            savedVolunteer!.FullName.Name.Should().Be("Иван");
            savedVolunteer.FullName.Surname.Should().Be("Иванов");
        });
    }

    [Fact]
    public async Task GetById_WithExistingVolunteer_ShouldReturnVolunteer()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var result = await dbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(volunteer.Id);
        });
    }

    [Fact]
    public async Task GetById_WithNonExistentVolunteer_ShouldReturnNull()
    {
        var nonExistentId = VolunteerId.Of(Guid.NewGuid());

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var result = await dbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == nonExistentId);

            result.Should().BeNull();
        });
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkAsDeleted()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var entity = await dbContext.Volunteers
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            entity!.Delete();
        });

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var softDeleted = await dbContext.Volunteers
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            softDeleted.Should().NotBeNull();
            softDeleted!.IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task AddPet_ShouldPersistPetWithVolunteer()
    {
        var volunteer = VolunteerBuilder.Default().Build();
        await InsertAsync(volunteer);

        var pet = PetBuilder.Default()
            .WithVolunteerId(volunteer.Id)
            .WithNickName("Барсик")
            .Build();

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var entity = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            entity!.AddPet(pet);
        });

        await ExecuteWithDbContextAsync(async dbContext =>
        {
            var volunteerWithPets = await dbContext.Volunteers
                .Include(v => v.Pets)
                .FirstOrDefaultAsync(v => v.Id == volunteer.Id);

            volunteerWithPets!.Pets.Should().HaveCount(1);
            volunteerWithPets.Pets.First().NickName.Value.Should().Be("Барсик");
        });
    }
}
