namespace PetFamily.SharedKernel.Domain.Primitives;

/// <summary>
/// Базовый тип команды, поддерживающий корреляцию сообщений.
/// </summary>
public class Command
{
    /// <summary>
    /// Уникальный идентификатор команды, связывает все события, вызванные этой командой.
    /// </summary>
    public Guid CorrelationId { get; } = Guid.NewGuid();

    /// <summary>
    /// Время создания команды (UTC).
    /// </summary>
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
}
