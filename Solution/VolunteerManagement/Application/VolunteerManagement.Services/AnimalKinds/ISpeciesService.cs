using VolunteerManagement.Domain.Aggregates.AnimalKinds;

namespace VolunteerManagement.Services.AnimalKinds;

/// <summary>
/// Интерфейс-сервис для работы с видами животных.
/// </summary>
public interface ISpeciesService
{
    /// <summary>
    /// Добавить новый вид животного.
    /// </summary>
    /// <param name="animalKind">Вид животного.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданного вида.</returns>
    Task<Guid> AddAsync(string animalKind, CancellationToken ct);

    /// <summary>
    /// Добавить породу к виду животного.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedName">Название породы.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Идентификатор созданной породы.</returns>
    Task<Guid> AddBreedAsync(Guid speciesId, string breedName, CancellationToken ct);

    /// <summary>
    /// Удалить вид животного (soft delete).
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    Task DeleteSpeciesAsync(Guid speciesId, CancellationToken ct);

    /// <summary>
    /// Удалить породу (soft delete).
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="ct">Токен отмены.</param>
    Task DeleteBreedAsync(Guid speciesId, Guid breedId, CancellationToken ct);

    /// <summary>
    /// Получить все виды животных.
    /// </summary>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Список всех видов с породами.</returns>
    Task<IReadOnlyList<Species>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Получить вид животного по идентификатору.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Вид животного с породами.</returns>
    Task<Species> GetByIdAsync(Guid speciesId, CancellationToken ct);

    /// <summary>
    /// Проверить существование породы в указанном виде.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>True, если порода существует и не удалена.</returns>
    Task<bool> ValidateBreedExistsAsync(Guid speciesId, Guid breedId, CancellationToken ct);

    /// <summary>
    /// Восстановить вид животного и все его породы.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RestoreSpeciesAsync(Guid speciesId, CancellationToken ct);

    /// <summary>
    /// Восстановить породу.
    /// </summary>
    /// <param name="speciesId">Идентификатор вида.</param>
    /// <param name="breedId">Идентификатор породы.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RestoreBreedAsync(Guid speciesId, Guid breedId, CancellationToken ct);
}