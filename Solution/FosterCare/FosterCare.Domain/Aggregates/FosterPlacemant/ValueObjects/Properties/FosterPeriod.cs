using FosterCare.Domain.Aggregates.FosterPlacemant.Enums;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Properties;

/// <summary>
/// Период передержки: дата начала, плановая дата окончания и фактическая дата окончания (опционально).
/// </summary>
public sealed record FosterPeriod
{
	private const int MinDurationDays = 7;
	private const int MaxDurationDays = 90;

	/// <summary>
	/// Дата начала передержки.
	/// </summary>
	public DateOnly StartDate { get; }

	/// <summary>
	/// Плановая дата окончания передержки.
	/// </summary>
	public DateOnly PlannedEndDate { get; }

	/// <summary>
	/// Фактическая дата окончания передержки (если передержка завершена).
	/// </summary>
	public DateOnly? ActualEndDate { get; }

	private FosterPeriod(DateOnly startDate, DateOnly plannedEndDate, DateOnly? actualEndDate)
	{
		StartDate = startDate;
		PlannedEndDate = plannedEndDate;
		ActualEndDate = actualEndDate;
	}

	/// <summary>
	/// Создаёт период передержки с валидацией инвариантов.
	/// </summary>
	/// <exception cref="DomainException">Выбрасывается при нарушении инвариантов.</exception>
	public static FosterPeriod Of(
		DateOnly startDate,
		DateOnly plannedEndDate,
		DateOnly? actualEndDate = null)
	{
		if (startDate == default)
		{
			throw new DomainException("Дата начала передержки обязательна.");
		}

		if (plannedEndDate == default)
		{
			throw new DomainException("Плановая дата окончания передержки обязательна.");
		}

		var durationDays = plannedEndDate.DayNumber - startDate.DayNumber;
		if (durationDays < MinDurationDays || durationDays > MaxDurationDays)
		{
			throw new DomainException($"Плановый срок передержки должен быть от {MinDurationDays} до {MaxDurationDays} дней.");
		}

		if (actualEndDate.HasValue && actualEndDate.Value == default)
		{
			throw new DomainException("Фактическая дата окончания передержки некорректна.");
		}

		if (actualEndDate.HasValue && actualEndDate.Value.DayNumber < startDate.DayNumber)
		{
			throw new DomainException("Фактическая дата окончания не может быть раньше даты начала передержки.");
		}

		return new FosterPeriod(startDate, plannedEndDate, actualEndDate);
	}
}