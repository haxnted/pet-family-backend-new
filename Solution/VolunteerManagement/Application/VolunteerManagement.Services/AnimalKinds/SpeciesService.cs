using PetFamily.SharedKernel.Infrastructure.Abstractions;
using VolunteerManagement.Domain.Aggregates.AnimalKinds;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.Entities;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Identifiers;
using VolunteerManagement.Domain.Aggregates.AnimalKinds.ValueObjects.Properties;
using VolunteerManagement.Services.AnimalKinds.Specifications;
using PetFamily.SharedKernel.Application.Exceptions;

namespace VolunteerManagement.Services.AnimalKinds;

/// <inheritdoc/>
/// <param name="repository">Репозиторий над видами животных.</param>
internal sealed class SpeciesService(IRepository<Species> repository) : ISpeciesService
{
    /// <inheritdoc/>
    public async Task<Guid> AddAsync(string animalKind, CancellationToken ct)
    {
        var animalKindValue = AnimalKind.Of(animalKind);

        var specification = new GetByAnimalKindSpecification(animalKindValue);

        var existingSpecies = await repository.FirstOrDefaultAsync(specification, ct);

        if (existingSpecies != null)
        {
            throw new UseCaseException($"Вид животного '{animalKind}' уже существует.");
        }

        var species = Species.Create(animalKindValue);

        await repository.AddAsync(species, ct);

        return species.Id.Value;
    }

    /// <inheritdoc/>
    public async Task<Guid> AddBreedAsync(Guid speciesId, string breedName, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        var breedNameValue = BreedName.Of(breedName);
        var breed = Breed.Create(breedNameValue, speciesIdValue);

        species.AddBreed(breed);

        await repository.UpdateAsync(species, ct);

        return breed.Id.Value;
    }

    /// <inheritdoc/>
    public async Task DeleteSpeciesAsync(Guid speciesId, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        species.Delete();

        await repository.UpdateAsync(species, ct);
    }

    /// <inheritdoc/>
    public async Task DeleteBreedAsync(Guid speciesId, Guid breedId, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        var breedIdValue = BreedId.Of(breedId);
        var breed = species.Breeds.FirstOrDefault(currentBreed =>
            currentBreed.Id == breedIdValue && currentBreed.DeletedAt == null);

        if (breed == null)
        {
            throw new UseCaseException($"Порода с идентификатором '{breedId}' не найдена в виде '{speciesId}'.");
        }

        breed.Delete();

        await repository.UpdateAsync(species, ct);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Species>> GetAllAsync(CancellationToken ct) =>
        repository.GetAll(new GetAllSpeciesSpecification(), ct);

    /// <inheritdoc/>
    public async Task<Species> GetByIdAsync(Guid speciesId, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);


        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        return species;
    }

    /// <inheritdoc/>
    public async Task<bool> ValidateBreedExistsAsync(
        Guid speciesId,
        Guid breedId,
        CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            return false;
        }

        var breedIdValue = BreedId.Of(breedId);

        return species.Breeds.Any(currentBreed =>
            currentBreed.Id == breedIdValue &&
            currentBreed.DeletedAt == null);
    }

    /// <inheritdoc/>
    public async Task RestoreSpeciesAsync(Guid speciesId, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        species.Restore();

        await repository.UpdateAsync(species, ct);
    }

    /// <inheritdoc/>
    public async Task RestoreBreedAsync(Guid speciesId, Guid breedId, CancellationToken ct)
    {
        var speciesIdValue = SpeciesId.Of(speciesId);

        var specification = new GetByIdSpecification(speciesIdValue);

        var species = await repository.FirstOrDefaultAsync(specification, ct);

        if (species == null)
        {
            throw new EntityNotFoundException<Species>(speciesId);
        }

        var breedIdValue = BreedId.Of(breedId);
        var breed = species.Breeds.FirstOrDefault(currentBreed => currentBreed.Id == breedIdValue);

        if (breed == null)
        {
            throw new UseCaseException($"Порода с идентификатором '{breedId}' не найдена в виде '{speciesId}'.");
        }

        breed.Restore();

        await repository.UpdateAsync(species, ct);
    }
}