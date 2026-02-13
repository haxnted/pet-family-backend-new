using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;
using VolunteerManagement.Tests.Domain.Builders;

namespace VolunteerManagement.Tests.Domain.Aggregates;

public sealed class SpeciesTests : UnitTestBase
{
    #region Create Tests

    [Fact]
    public void Create_ShouldCreateSpecies()
    {
        var species = SpeciesBuilder.Default().Build();

        species.Should().NotBeNull();
        species.AnimalKind.Value.Should().Be("Собака");
        species.Breeds.Should().BeEmpty();
    }

    #endregion

    #region AddBreed Tests

    [Fact]
    public void AddBreed_ShouldAddToCollection()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed = Breed.Create(BreedName.Of("Лабрадор"), species.Id);

        species.AddBreed(breed);

        species.Breeds.Should().HaveCount(1);
        species.Breeds.Should().Contain(breed);
    }

    [Fact]
    public void AddBreed_WithDuplicateName_ShouldThrow()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed1 = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        var breed2 = Breed.Create(BreedName.Of("Лабрадор"), species.Id);

        species.AddBreed(breed1);
        var act = () => species.AddBreed(breed2);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddBreed_MultipleDifferent_ShouldAddAll()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed1 = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        var breed2 = Breed.Create(BreedName.Of("Овчарка"), species.Id);

        species.AddBreed(breed1);
        species.AddBreed(breed2);

        species.Breeds.Should().HaveCount(2);
    }

    #endregion

    #region RemoveBreed Tests

    [Fact]
    public void RemoveBreed_ShouldRemoveFromCollection()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        species.AddBreed(breed);

        species.RemoveBreed(breed.Id);

        species.Breeds.Should().BeEmpty();
    }

    [Fact]
    public void RemoveBreed_WithNonExistent_ShouldThrow()
    {
        var species = SpeciesBuilder.Default().Build();

        var act = () => species.RemoveBreed(BreedId.Of(Guid.NewGuid()));

        act.Should().Throw<DomainException>();
    }

    #endregion

    #region Delete/Restore Tests

    [Fact]
    public void Delete_ShouldCascadeToBreeds()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed1 = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        var breed2 = Breed.Create(BreedName.Of("Овчарка"), species.Id);
        species.AddBreed(breed1);
        species.AddBreed(breed2);

        species.Delete();

        species.IsDeleted.Should().BeTrue();
        species.Breeds.Should().AllSatisfy(b => b.IsDeleted.Should().BeTrue());
    }

    [Fact]
    public void Restore_ShouldCascadeToBreeds()
    {
        var species = SpeciesBuilder.Default().Build();
        var breed = Breed.Create(BreedName.Of("Лабрадор"), species.Id);
        species.AddBreed(breed);
        species.Delete();

        species.Restore();

        species.IsDeleted.Should().BeFalse();
        species.Breeds.Should().AllSatisfy(b => b.IsDeleted.Should().BeFalse());
    }

    #endregion
}
