using Ardalis.Specification;

namespace PetFamily.SharedKernel.Infrastructure.Abstractions;

/// <summary>
/// Репозиторий для доступа к данным.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Добавляет сущность в хранилище.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AddAsync(TEntity entity, CancellationToken ct);

    /// <summary>
    /// Добавляет список сущностей в хранилище.
    /// </summary>
    /// <param name="entities">Сущности.</param>
    /// <param name="ct">Токен отмены.</param>
    Task AddRangeAsync(TEntity[] entities, CancellationToken ct);

    /// <summary>
    /// Возвращает первую сущность, соответствующую спецификации.
    /// </summary>
    /// <typeparam name="TSpec">Тип спецификации.</typeparam>
    /// <param name="specification">Спецификация.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Первая сущность или null.</returns>
    Task<TEntity?> FirstOrDefaultAsync<TSpec>(TSpec specification, CancellationToken ct)
        where TSpec : ISpecification<TEntity>;
    
    /// <summary>
    /// Возвращает все сущности, соответствующую спецификации.
    /// </summary>
    /// <typeparam name="TSpec">Тип спецификации.</typeparam>
    /// <param name="specification">Спецификация.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Первая сущность или null.</returns>
    Task<IReadOnlyList<TEntity>> GetAll<TSpec>(TSpec specification, CancellationToken ct)
        where TSpec : ISpecification<TEntity>;

    /// <summary>
    /// Обновляет данные сущности.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <param name="ct">Токен отмены.</param>
    Task UpdateAsync(TEntity entity, CancellationToken ct);

    /// <summary>
    /// Удаляет сущность из хранилища.
    /// </summary>
    /// <param name="entity">Сущность.</param>
    /// <param name="ct">Токен отмены.</param>
    Task RemoveAsync(TEntity entity, CancellationToken ct);
}