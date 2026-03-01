namespace FosterCare.Domain.Aggregates.FosterPlacemant.Enums;

/// <summary>
/// Статус передержки (жизненный цикл заявки/размещения).
/// </summary>
public enum FosterStatus
{
	/// <summary>Создана, ожидает одобрения администратором.</summary>
	PendingApproval,

	/// <summary>Одобрена, ожидает фактического размещения.</summary>
	Approved,

	/// <summary>Запущен процесс размещения (saga/оркестрация).</summary>
	Placing,

	/// <summary>Питомец находится на передержке у волонтёра.</summary>
	Active,

	/// <summary>Запущен процесс возврата питомца в приют.</summary>
	ReturningToShelter,

	/// <summary>Передержка завершена, питомец возвращён.</summary>
	Completed,

	/// <summary>Питомец усыновлён из передержки.</summary>
	AdoptedFromFoster,

	/// <summary>Передержка отменена (отказ/компенсация/прочие причины).</summary>
	Cancelled
}