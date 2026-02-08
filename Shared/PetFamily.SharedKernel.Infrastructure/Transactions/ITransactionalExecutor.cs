using System.Data;

namespace PetFamily.SharedKernel.Infrastructure.Transactions;

/// <summary>
/// Интерфейс для выполнения операций в транзакции с заданным уровнем изоляции.
/// </summary>
public interface ITransactionalExecutor
{
    /// <summary>
    /// Выполняет указанное действие внутри транзакции с заданным уровнем изоляции.
    /// </summary>
    /// <param name="action">Асинхронное действие, выполняемое внутри транзакции.</param>
    /// <param name="isolationLevel">Уровень изоляции транзакции.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task StartEffect(Func<CancellationToken, Task> action, IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
}