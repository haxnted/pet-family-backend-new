using PetFamil.SharedKernel.Common.Abstractions;

namespace PetFamil.SharedKernel.Common;

internal class UtcCurrentMoment : ICurrentMoment
{
	public DateTime Now => DateTime.UtcNow;
}