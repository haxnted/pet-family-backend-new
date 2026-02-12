using MassTransit;

namespace VolunteerManagement.Infrastructure.SagaStates;

/// <summary>
/// Состояние саги усыновления питомца.
/// Хранит контекст текущего экземпляра саги и персистится в БД.
/// </summary>
public class PetAdoptionState : SagaStateMachineInstance
{
    /// <summary>
    /// Идентификатор корреляции (уникальный для каждого запуска саги).
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Текущее состояние саги (строковое представление).
    /// </summary>
    public string CurrentState { get; set; } = null!;

    /// <summary>
    /// Идентификатор питомца.
    /// </summary>
    public Guid PetId { get; set; }

    /// <summary>
    /// Идентификатор волонтёра.
    /// </summary>
    public Guid VolunteerId { get; set; }

    /// <summary>
    /// Идентификатор усыновителя.
    /// </summary>
    public Guid AdopterId { get; set; }

    /// <summary>
    /// Имя усыновителя.
    /// </summary>
    public string AdopterName { get; set; } = null!;

    /// <summary>
    /// Кличка питомца.
    /// </summary>
    public string PetNickName { get; set; } = null!;

    /// <summary>
    /// Идентификатор созданного чата (заполняется после CreatingChat).
    /// </summary>
    public Guid? ChatId { get; set; }

    /// <summary>
    /// Дата начала саги.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Причина сбоя (если сага завершилась с ошибкой).
    /// </summary>
    public string? FailureReason { get; set; }
}
