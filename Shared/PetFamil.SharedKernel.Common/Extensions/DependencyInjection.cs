using Microsoft.Extensions.DependencyInjection;
using PetFamil.SharedKernel.Common.Abstractions;
using PetFamil.SharedKernel.Common.Utils;

namespace PetFamil.SharedKernel.Common.Extensions;

public static class DependencyInjection
{
	public static IServiceCollection AddCommonLayerDependencies(this IServiceCollection services)
	{
		services.AddSingleton<ICurrentMoment, UtcCurrentMoment>();

		services.AddSingleton<DateTimeHelper>(sp =>
		{
			var dateTimeProvider = sp.GetRequiredService<ICurrentMoment>();
			DateTimeHelper.SetCurrentMomentProvider(dateTimeProvider);

			return new();
		});

		return services;
	}
}