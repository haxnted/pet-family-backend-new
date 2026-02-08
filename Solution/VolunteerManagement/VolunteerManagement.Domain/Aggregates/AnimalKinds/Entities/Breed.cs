using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;

/// <summary>
/// Сущность Порода животного.
/// </summary>
public sealed class Breed : SoftDeletableEntity<BreedId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private Breed(BreedId id) : base(id)
    {
    }

    /// <summary>
    /// Конструктор для создания породы.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Название породы.</param>
    /// <param name="speciesId">Идентификатор вида животного.</param>
    private Breed(BreedId id, BreedName name, SpeciesId speciesId) : base(id)
    {
        Name = name;
        SpeciesId = speciesId;
    }

    /// <summary>
    /// Название породы.
    /// </summary>
    public BreedName Name { get; private set; } = null!;

    /// <summary>
    /// Идентификатор вида животного.
    /// </summary>
    public SpeciesId SpeciesId { get; private set; }

    /// <summary>
    /// Фабричный метод для создания породы.
    /// </summary>
    /// <param name="name">Название породы.</param>
    /// <param name="speciesId">Идентификатор вида животного.</param>
    /// <returns>Новый экземпляр породы.</returns>
    public static Breed Create(BreedName name, SpeciesId speciesId)
    {
        var id = BreedId.Of(Guid.NewGuid());
        return new Breed(id, name, speciesId);
    }
}