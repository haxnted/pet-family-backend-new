namespace PetFamily.SharedKernel.Infrastructure.Options;

/// <summary>
/// Настройки подключения к RabbitMQ.
/// </summary>
public class RabbitMqSettings
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public const string SectionName = "RabbitMQ";

    /// <summary>
    /// Хост RabbitMQ.
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// Порт RabbitMQ.
    /// </summary>
    public required ushort Port { get; init; }

    /// <summary>
    /// Виртуальный хост.
    /// </summary>
    public required string VirtualHost { get; init; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public required string Password { get; init; }
}