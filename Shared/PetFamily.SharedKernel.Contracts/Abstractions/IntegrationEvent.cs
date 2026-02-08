namespace PetFamily.SharedKernel.Contracts.Abstractions;

/// <summary>
/// Интерфейс для событий интеграции (отправляемых наружу, между сервисами).
/// </summary>
public abstract class IntegrationEvent
{
    /// <summary>
    /// Идентификатор корреляции, связывающий это событие с другими событиями.
    /// </summary>
    public Guid CorrelationId { get; } = Guid.NewGuid();

    /// <summary>
    /// Момент создания события в формате UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}