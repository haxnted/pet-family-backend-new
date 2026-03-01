using System.Diagnostics.CodeAnalysis;
using PetFamily.SharedKernel.Domain.Exceptions;

namespace FosterCare.Domain.Aggregates.FosterPlacemant.ValueObjects.Idenifiers;

public readonly struct FosterReportId :
	IEquatable<FosterReportId>,
	IComparable<FosterReportId>
{
	public Guid Value { get; }

	private FosterReportId(Guid id)
	{
		Value = id;
	}

	public static FosterReportId Of(Guid id)
	{
		if (id == Guid.Empty)
		{
			throw new DomainException("id не может быть пустым.");
		}

		return new FosterReportId(id);
	}

	public int CompareTo(FosterReportId other)
	{
		return Value.CompareTo(other.Value);
	}

	public bool Equals(FosterReportId other)
	{
		return Value == other.Value;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		return obj is FosterReportId id && Equals(id);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}

	public static bool operator ==(FosterReportId left, FosterReportId right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FosterReportId left, FosterReportId right)
	{
		return !(left == right);
	}

	public static bool operator <(FosterReportId left, FosterReportId right)
	{
		return left.CompareTo(right) < 0;
	}

	public static bool operator <=(FosterReportId left, FosterReportId right)
	{
		return left.CompareTo(right) <= 0;
	}

	public static bool operator >(FosterReportId left, FosterReportId right)
	{
		return left.CompareTo(right) > 0;
	}

	public static bool operator >=(FosterReportId left, FosterReportId right)
	{
		return left.CompareTo(right) >= 0;
	}
}