namespace PetFamily.SharedKernel.Infrastructure;

/// <summary>
/// Контракт, который предоставляет применение миграций.
/// </summary>
public interface IMigrator
{
    Task Migrate(CancellationToken cancellationToken);
}