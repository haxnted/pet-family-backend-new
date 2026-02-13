using PetFamily.SharedKernel.Tests.Abstractions;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

namespace VolunteerManagement.Tests.Domain.Builders;

/// <summary>
/// Builder для создания тестовых экземпляров Species.
/// </summary>
public sealed class SpeciesBuilder : IBuilder<Species>
{
    private AnimalKind _animalKind = AnimalKind.Of("Собака");

    public SpeciesBuilder WithAnimalKind(string kind)
    {
        _animalKind = AnimalKind.Of(kind);
        return this;
    }

    public Species Build()
    {
        return Species.Create(_animalKind);
    }

    public static SpeciesBuilder Default() => new();

    public static IReadOnlyList<Species> BuildMany(int count)
    {
        var kinds = new[] { "Собака", "Кошка", "Попугай", "Хомяк", "Кролик", "Черепаха", "Рыбка", "Ящерица" };
        return Enumerable.Range(0, count)
            .Select(i => new SpeciesBuilder().WithAnimalKind(kinds[i % kinds.Length]).Build())
            .ToList();
    }
}
