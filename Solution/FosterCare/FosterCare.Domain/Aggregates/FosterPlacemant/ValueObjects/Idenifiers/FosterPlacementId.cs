using PetFamily.SharedKernel.Domain.Exceptions;

namespace FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Idenifiers;

public readonly struct FosterPlacementId :
	IEquatable<FosterPlacementId>,
	IComparable<FosterPlacementId>
{
	public Guid Value { get; }

	private FosterPlacementId(Guid id)
	{
		Value = id;
	}

	public static FosterPlacementId Of(Guid id)
	{
		if (id == Guid.Empty)
		{
			throw new DomainException("id не может быть пустым.");
		}

		return new FosterPlacementId(id);
	}

	public bool Equals(FosterPlacementId other)
	{
		return Value == other.Value;
	}

	public override bool Equals(object? obj)
	{
		return obj is FosterPlacementId id && Equals(id);
	}

	public int CompareTo(FosterPlacementId other)
	{
		return Value.CompareTo(other.Value);
	}

	public static bool operator ==(FosterPlacementId first, FosterPlacementId second)
	{
		return first.Equals(second);
	}

	public static bool operator !=(FosterPlacementId first, FosterPlacementId second)
	{
		return !(first == second);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	public static bool operator <(FosterPlacementId left, FosterPlacementId right)
	{
		return left.CompareTo(right) < 0;
	}

	public static bool operator <=(FosterPlacementId left, FosterPlacementId right)
	{
		return left.CompareTo(right) <= 0;
	}

	public static bool operator >(FosterPlacementId left, FosterPlacementId right)
	{
		return left.CompareTo(right) > 0;
	}

	public static bool operator >=(FosterPlacementId left, FosterPlacementId right)
	{
		return left.CompareTo(right) >= 0;
	}
}