using PetFamil.SharedKernel.Common.Abstractions;

namespace PetFamil.SharedKernel.Common.Utils;

public sealed class DateTimeHelper
{
	private static ICurrentMoment? _currentMomentProvider;

	public static DateTime Now => _currentMomentProvider?.Now
		?? throw new InvalidOperationException(
			"Необходимо установить провайдер текущего времени перед вызовом. " +
			$"Используйте метод '{nameof(SetCurrentMomentProvider)}'."
		);

	public static void SetCurrentMomentProvider(ICurrentMoment provider)
	{
		ArgumentNullException.ThrowIfNull(provider, nameof(provider));

		if (_currentMomentProvider is not null)
		{
			throw new InvalidOperationException("Провайдер текущего времени уже установлен.");
		}

		_currentMomentProvider = provider;
	}
}