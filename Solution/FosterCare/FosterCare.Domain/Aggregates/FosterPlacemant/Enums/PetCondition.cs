namespace FosterCare.Domain.Aggregates.FosterPlacemant.Enums;

/// <summary>
/// Оценка состояния питомца.
/// </summary>
public enum PetCondition
{
	/// <summary>Отличное состояние.</summary>
	Excellent,

	/// <summary>Хорошее состояние.</summary>
	Good,

	/// <summary>Есть поводы для беспокойства (требует внимания/контроля).</summary>
	Concerning,

	/// <summary>Критическое состояние (должно триггерить экстренное уведомление).</summary>
	Critical
}
