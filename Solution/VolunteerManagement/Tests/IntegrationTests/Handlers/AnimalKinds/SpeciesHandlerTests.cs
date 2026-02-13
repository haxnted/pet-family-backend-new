using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;
using VolunteerManagement.Services.AnimalKinds;
using VolunteerManagement.Tests.Domain.Builders;
using VolunteerManagement.Tests.Integration.Abstractions;
using VolunteerManagement.Tests.Integration.Fixtures;

namespace VolunteerManagement.Tests.Integration.Handlers.AnimalKinds;

[Collection(VolunteerManagementIntegrationTestCollection.Name)]
public sealed class SpeciesHandlerTests(VolunteerManagementWebApplicationFactory factory)
    : VolunteerManagementIntegrationTestBase(factory)
{
    [Fact]
    public async Task AddSpecies_ShouldPersist()
    {
        // Act
        Guid speciesId = Guid.Empty;
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            speciesId = await service.AddAsync("Кошка", CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var species = await db.Species.FirstOrDefaultAsync();
            species.Should().NotBeNull();
            species!.AnimalKind.Value.Should().Be("Кошка");
        });
    }

    [Fact]
    public async Task AddBreed_ShouldAddToSpecies()
    {
        // Arrange
        var species = SpeciesBuilder.Default().Build();
        await InsertAsync(species);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            await service.AddBreedAsync(species.Id.Value, "Мейн-кун", CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == species.Id);

            found!.Breeds.Should().HaveCount(1);
        });
    }

    [Fact]
    public async Task DeleteSpecies_ShouldSoftDelete()
    {
        // Arrange
        var species = SpeciesBuilder.Default().Build();
        var breed = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        species.AddBreed(breed);
        await InsertAsync(species);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            await service.DeleteSpeciesAsync(species.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Species
                .IgnoreQueryFilters()
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == species.Id);

            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task DeleteBreed_ShouldSoftDeleteBreed()
    {
        // Arrange
        var species = SpeciesBuilder.Default().Build();
        var breed = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        species.AddBreed(breed);
        await InsertAsync(species);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            await service.DeleteBreedAsync(species.Id.Value, breed.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Species
                .Include(s => s.Breeds)
                .FirstOrDefaultAsync(s => s.Id == species.Id);

            found.Should().NotBeNull();
            found!.Breeds.Should().HaveCount(1);
            found.Breeds.First().IsDeleted.Should().BeTrue();
        });
    }

    [Fact]
    public async Task RestoreSpecies_ShouldRestore()
    {
        // Arrange
        var species = SpeciesBuilder.Default().Build();
        species.Delete();
        await InsertAsync(species);

        // Act
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            await service.RestoreSpeciesAsync(species.Id.Value, CancellationToken.None);
        });

        // Assert
        await ExecuteWithDbContextAsync(async db =>
        {
            var found = await db.Species.FirstOrDefaultAsync(s => s.Id == species.Id);
            found.Should().NotBeNull();
            found!.IsDeleted.Should().BeFalse();
        });
    }

    [Fact]
    public async Task GetAllSpecies_ShouldReturnAll()
    {
        // Arrange
        await InsertAsync(SpeciesBuilder.Default().WithAnimalKind("Собака").Build());
        await InsertAsync(SpeciesBuilder.Default().WithAnimalKind("Кошка").Build());

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            var result = await service.GetAllAsync(CancellationToken.None);

            result.Should().HaveCount(2);
        });
    }

    [Fact]
    public async Task GetSpeciesById_ShouldReturnSpecies()
    {
        // Arrange
        var species = SpeciesBuilder.Default().Build();
        await InsertAsync(species);

        // Act & Assert
        await ExecuteInScopeAsync(async sp =>
        {
            var service = sp.GetRequiredService<ISpeciesService>();
            var result = await service.GetByIdAsync(species.Id.Value, CancellationToken.None);

            result.Should().NotBeNull();
            result.AnimalKind.Value.Should().Be("Собака");
        });
    }
}