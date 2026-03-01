namespace FosterCare.Domain.Aggregates.FosterPlacemant.Enums;

/// <summary>
/// Причина, по которой питомец отправлен на передержку.
/// </summary>
public enum FosterReason
{
	/// <summary>Переполнение приюта (нужно разгрузить места).</summary>
	ShelterOvercrowding,

	/// <summary>Восстановление после лечения/операции.</summary>
	MedicalRecovery,

	/// <summary>Социализация в домашних условиях.</summary>
	Socialization,

	/// <summary>Временный уход (ремонт приюта, карантин и т.п.).</summary>
	TemporaryCare
}