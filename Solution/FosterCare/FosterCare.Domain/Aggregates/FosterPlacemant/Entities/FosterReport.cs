using FosterCare.Domain.Aggregates.FosterPlacemant.Enums;
using FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Idenifiers;
using PetFamil.SharedKernel.Common.Utils;
using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace FosterCare.Domain.Aggregates.FosterPlacemant.Entities;

/// <summary>
/// Отчёт волонтёра по передержке за конкретную дату.
/// </summary>
public class FosterReport :
	Entity<FosterReportId>
{
	private const int TextMinLength = 10;
	private const int TextMaxLength = 2000;

	/// <summary>
	/// Идентификатор передержки, к которой относится отчёт.
	/// </summary>
	public FosterPlacementId PlacementId { get; private set; }

	/// <summary>
	/// Текст отчёта (10..2000 символов).
	/// </summary>
	public string Text { get; private set; }

	/// <summary>
	/// Оценка состояния питомца на дату отчёта.
	/// </summary>
	public PetCondition Condition { get; private set; }

	/// <summary>
	/// Идентификаторы файлов в хранилище (фото).
	/// </summary>
	public List<Guid> PhotoIds { get; private set; }

	/// <summary>
	/// Дата, за которую составлен отчёт.
	/// </summary>
	public DateOnly ReportDate { get; private set; }

	/// <summary>
	/// Момент создания отчёта.
	/// </summary>
	public DateTime CreatedAt { get; private set; }

	private FosterReport(FosterReportId id) : base(id) { }

	private FosterReport(
		FosterReportId id,
		FosterPlacementId placementId,
		string text,
		PetCondition condition,
		List<Guid> photoIds,
		DateOnly reportDate,
		DateTime createdAt) : base(id)
	{
		PlacementId = placementId;
		Text = text;
		Condition = condition;
		PhotoIds = photoIds;
		ReportDate = reportDate;
		CreatedAt = createdAt;
	}

	/// <summary>
	/// Создаёт новый отчёт по передержке с валидацией инвариантов домена.
	/// </summary>
	/// <exception cref="DomainException">Выбрасывается при нарушении инвариантов.</exception>
	public static FosterReport Create(
		FosterReportId id,
		FosterPlacementId placementId,
		string text,
		PetCondition condition,
		IEnumerable<Guid> photoIds,
		DateOnly reportDate,
		DateTime? createdAt = null)
	{
		if (text is null)
		{
			throw new DomainException("Текст отчёта обязателен.");
		}

		var normalizedText = text.Trim();
		if (normalizedText.Length < TextMinLength || normalizedText.Length > TextMaxLength)
		{
			throw new DomainException($"Длина текста отчёта должна быть от {TextMinLength} до {TextMaxLength} символов.");
		}

		if (reportDate == default)
		{
			throw new DomainException("Дата отчёта обязательна.");
		}

		var utcCreatedAt = createdAt ?? DateTimeHelper.Now;

		return new FosterReport(
			id,
			placementId,
			normalizedText,
			condition,
			photoIds.ToList(),
			reportDate,
			utcCreatedAt
		);
	}
}