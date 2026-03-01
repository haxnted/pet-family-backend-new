using FosterCare.Domain.Aggregates.FosterPlacemant.Enums;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Properties;

/// <summary>
/// Условия передержки в доме волонтёра.
/// </summary>
public sealed record FosterConditions
{
	private const int AdditionalInfoMaxLength = 1000;

	/// <summary>
	/// Наличие других животных в доме.
	/// </summary>
	public bool HasOtherPets { get; }

	/// <summary>
	/// Наличие детей в доме.
	/// </summary>
	public bool HasChildren { get; }

	/// <summary>
	/// Тип жилья волонтёра.
	/// </summary>
	public HomeType HomeType { get; }

	/// <summary>
	/// Дополнительная информация (до 1000 символов).
	/// </summary>
	public string? AdditionalInfo { get; }

	private FosterConditions(bool hasOtherPets, bool hasChildren, HomeType homeType, string? additionalInfo)
	{
		HasOtherPets = hasOtherPets;
		HasChildren = hasChildren;
		HomeType = homeType;
		AdditionalInfo = additionalInfo;
	}

	/// <summary>
	/// Создаёт условия передержки с валидацией инвариантов.
	/// </summary>
	/// <exception cref="DomainException">Выбрасывается при нарушении инвариантов.</exception>
	public static FosterConditions Of(
		bool hasOtherPets,
		bool hasChildren,
		HomeType homeType,
		string? additionalInfo)
	{
		var normalizedInfo = additionalInfo?.Trim();
		if (string.IsNullOrWhiteSpace(normalizedInfo))
		{
			normalizedInfo = null;
		}

		if (normalizedInfo is not null && normalizedInfo.Length > AdditionalInfoMaxLength)
		{
			throw new DomainException($"Дополнительная информация не должна превышать {AdditionalInfoMaxLength} символов.");
		}

		return new FosterConditions(hasOtherPets, hasChildren, homeType, normalizedInfo);
	}
}