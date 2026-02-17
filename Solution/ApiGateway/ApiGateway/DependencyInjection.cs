using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using PetFamily.SharedKernel.WebApi.Extensions;

namespace ApiGateway;

/// <summary>
/// Конфигурация внедрения зависимостей для API Gateway.
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Добавить все зависимости программы.
	/// </summary>
	/// <param name="services">Коллекция сервисов.</param>
	/// <param name="configuration">Конфигурация приложения.</param>
	public static IServiceCollection AddProgramDependencies(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddEndpointsApiExplorer();

		services.AddKeycloakJwtBearer(configuration, mapKeycloakRoles: false);
		services.AddAuthorization();
		services.AddAllowAllCors("AllowAll");

		services.ConfigureOcelot(configuration);

		services.AddStandardHealthChecks()
			.AddKeycloakHealthCheck(configuration, "Keycloak");

		return services;
	}

	/// <summary>
	/// Настроить Ocelot с кэшированием и Polly.
	/// </summary>
	/// <param name="services">Коллекция сервисов.</param>
	/// <param name="configuration">Конфигурация приложения.</param>
	private static IServiceCollection ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOcelot(configuration)
			.AddCacheManager(settings =>
			{
				settings.WithDictionaryHandle();
			})
			.AddPolly();

		return services;
	}
}