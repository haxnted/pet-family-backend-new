using PetFamily.SharedKernel.Domain.Primitives;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace VolunteerManagement.Domain.Aggregates.AnimalKinds;

/// <summary>
/// Агрегат-корень "Вид".
/// </summary>
public sealed class Species : SoftDeletableEntity<SpeciesId>
{
    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    private Species(SpeciesId id) : base(id)
    {
    }

    /// <summary>
    /// EF Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="animalKind">Вид Животного.</param>
    private Species(SpeciesId id, AnimalKind animalKind) : base(id)
    {
        AnimalKind = animalKind;
    }

    /// <summary>
    /// Фабричный метод для создания вида животного.
    /// </summary>
    /// <param name="animalKind">Вид животного.</param>
    /// <returns>Новый экземпляр вида животного.</returns>
    public static Species Create(AnimalKind animalKind)
    {
        var id = SpeciesId.Of(Guid.NewGuid());
        return new Species(id, animalKind);
    }

    /// <summary>
    /// Вид Животного.
    /// </summary>
    public AnimalKind AnimalKind { get; private set; } = null!;

    /// <summary>
    /// Породы животного.
    /// </summary>
    public IReadOnlyCollection<Breed> Breeds => _breeds;

    /// <summary>
    /// Породы животного.
    /// </summary>
    private readonly List<Breed> _breeds = [];

    /// <summary>
    /// Добавить породу.
    /// </summary>
    /// <param name="breed">Порода животного.</param>
    public void AddBreed(Breed breed)
    {
        if (_breeds.Any(b => b.Name == breed.Name))
        {
            throw new DomainException("Такая порода уже существует.");
        }

        _breeds.Add(breed);
    }

    /// <summary>
    /// Удалить породу.
    /// </summary>
    /// <param name="breedId">Порода животного.</param>
    public void RemoveBreed(BreedId breedId)
    {
        var existing = _breeds.FirstOrDefault(b => b.Id == breedId);
        if (existing == null)
        {
            throw new DomainException("Такая порода не существует.");
        }

        _breeds.Remove(existing);
    }

    /// <summary>
    /// Переопределяет базовый метод удаления для каскадного удаления всех пород.
    /// </summary>
    public override void Delete()
    {
        base.Delete();
        foreach (var breed in _breeds)
        {
            breed.Delete();
        }
    }

    /// <summary>
    /// Переопределяет базовый метод восстановления для каскадного восстановления всех пород.
    /// </summary>
    public override void Restore()
    {
        base.Restore();
        foreach (var breed in _breeds)
        {
            breed.Restore();
        }
    }
}