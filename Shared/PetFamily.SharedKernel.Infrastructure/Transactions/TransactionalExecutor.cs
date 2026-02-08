using System.Data;
using Microsoft.EntityFrameworkCore;

namespace PetFamily.SharedKernel.Infrastructure.Transactions;

/// <summary>
/// Базовая реализация <see cref="ITransactionalExecutor"/> для <see cref="DbContext"/>.
/// </summary>
/// <typeparam name="TContext">Тип контекста базы данных.</typeparam>
/// <remarks>
/// Конструктор.
/// </remarks>
/// <param name="dbContext">Экземпляр DbContext, с которым будет работать транзакция.</param>
public class TransactionalExecutor<TContext>(TContext dbContext) : ITransactionalExecutor
    where TContext : DbContext
{
    private readonly TContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task StartEffect(Func<CancellationToken, Task> action, IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (_dbContext.Database.CurrentTransaction != null)
        {
            await action(cancellationToken);
            return;
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            try
            {
                await action(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}