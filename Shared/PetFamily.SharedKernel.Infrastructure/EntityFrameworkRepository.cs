using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetFamily.SharedKernel.Infrastructure.Abstractions;

namespace PetFamily.SharedKernel.Infrastructure;

/// <summary>
/// Реализация репозитория на Entity Framework Core.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public sealed class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private DbContext DbContext { get; }

    /// <summary>
    /// Коллекция сущностей.
    /// </summary>
    private DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных.</param>
    public EntityFrameworkRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.AddRangeAsync(entities, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity?> FirstOrDefaultAsync<TSpec>(TSpec specification, CancellationToken cancellationToken)
        where TSpec : ISpecification<TEntity>
    {
        ArgumentNullException.ThrowIfNull(specification);

        return await DbSet.WithSpecification(specification)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> GetAll<TSpec>(TSpec specification, CancellationToken cancellationToken)
        where TSpec : ISpecification<TEntity>
    {
        return await DbSet.WithSpecification(specification)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbContext.Update(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbContext.Remove(entity);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
