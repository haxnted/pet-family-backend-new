using FosterCare.Domain.Aggregates.FosterPlacemant.Entities;
using FosterCare.Domain.Aggregates.FosterPlacemant.Enums;
using FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Idenifiers;
using FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Properties;
using PetFamily.SharedKernel.Domain.Exceptions;
using PetFamily.SharedKernel.Domain.Primitives;

namespace FosterCare.Domain.Aggregates.FosterPlacemant;

public class FosterPlacement :
	SoftDeletableEntity<FosterPlacementId>
{
	public Guid PetId { get; private set; }

	public Guid VolunteerId { get; private set; }

	public Guid ShelterId { get; private set; }

	public Guid? AdminId { get; private set; }

	public FosterStatus Status { get; private set; }

	public FosterReason Reason { get; private set; }

	public FosterPeriod Period { get; private set; }

	public FosterConditions Conditions { get; private set; }

	public string Experience { get; private set; }

	public string? Comment { get; private set; }

	public List<Guid> PhotoIds { get; private set; }

	public Guid? ChatId { get; private set; }

	public string? RejectionReason { get; private set; }

	public string? CancellationReason { get; private set; }

	private readonly List<FosterReport> _reports = [];

	private FosterPlacement(FosterPlacementId id) : base(id)
	{
	}

	public IReadOnlyList<FosterReport> Reports => _reports;

	public void Approve(Guid adminId)
	{
		if (Status != FosterStatus.PendingApproval)
		{
			throw new DomainException($"Подтвердить заявку можно только из статуса '{nameof(FosterStatus.PendingApproval)}'.");
		}

		Status = FosterStatus.Approved;
		AdminId = adminId;
	}

	public void Reject(Guid adminId, string reason)
	{
		if (Status != FosterStatus.PendingApproval)
		{
			throw new DomainException($"Отклонить заявку можно только из статуса '{nameof(FosterStatus.PendingApproval)}'.");
		}

		Status = FosterStatus.Cancelled;
		AdminId = adminId;
		RejectionReason = reason;
	}

	public void StartPlacement()
	{
		if (Status != FosterStatus.Approved)
		{
			throw new DomainException("Начать размещение можно только после одобрения заявки администратором");
		}

		Status = FosterStatus.Placing;
	}

	public void CompletePlacement(Guid chatId)
	{
		if (chatId == Guid.Empty)
		{
			throw new InvalidOperationException("Недействительный id чата.");
		}

		if (Status != FosterStatus.Placing)
		{
			throw new DomainException("Перевести заявку в активное состояние можно толко после её размещения");
		}

		Status = FosterStatus.Active;
		ChatId = chatId;
	}

	public void CancelPlacement(string reason)
	{
		if (string.IsNullOrEmpty(reason))
		{
			throw new DomainException("Необходимо указать причину отмены заявки.");
		}

		if (Status != FosterStatus.Placing)
		{
			throw new DomainException("Отменить заявку можно толко после её успешного размещения");
		}

		Status = FosterStatus.Cancelled;
		CancellationReason = reason;
	}

	public void AddReport(FosterReport report)
	{
		ArgumentNullException.ThrowIfNull(report, nameof(report));

		if (Status != FosterStatus.Active)
		{
			throw new DomainException("Невозможно отчитаться по закрытой заявке.");
		}

		_reports.Add(report);
	}

	public void InitiateReturn()
	{
		if (Status != FosterStatus.Active)
		{
			throw new DomainException(
				"Инициировать процесс передачи питомца с передержки можно " +
				"только при активной заявке."
			);
		}

		Status = FosterStatus.ReturningToShelter;
	}

	public void CompleteReturn()
	{
		if (Status != FosterStatus.ReturningToShelter)
		{
			throw new DomainException(
				"Перед завершением возврата питомца " +
				"необходимо инитиализировать процесс передачи."
			);
		}

		Status = FosterStatus.Completed;
	}

	public void ConvertToAdoption()
	{
		if (Status != FosterStatus.Active)
		{
			throw new DomainException("Невозможно усыновить питоца из неактивной передержки.");
		}

		Status = FosterStatus.AdoptedFromFoster;
	}

	public void ExtendPeriod(int days)
	{
		if (Status != FosterStatus.Active)
		{
			throw new DomainException("Невозможно продлить передержку для неактивного заявления.");
		}

		var extendedPeriod = FosterPeriod.Of(
			startDate: Period.StartDate,
			plannedEndDate: Period.PlannedEndDate.AddDays(days)
		);

		Period = extendedPeriod;
	}

	public void SetChatId(Guid chatId)
	{
		if (chatId == Guid.Empty)
		{
			throw new InvalidOperationException("Недействительный id чата.");
		}

		ChatId = chatId;
	}
}