using Microsoft.Extensions.DependencyInjection;

namespace Conversation.Services;

/// <summary>
/// Регистрация зависимостей слоя Application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавить зависимости Application.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}