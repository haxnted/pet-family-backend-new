namespace Notification.Infrastructure.Settings;

/// <summary>
/// SMTP configuration options for email sending.
/// </summary>
public class SmtpOptions
{
    /// <summary>
    /// Название секции в конфигурации.
    /// </summary>
    public const string SectionName = "Smtp";

    /// <summary>
    /// Хост.
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// Порт.
    /// </summary>
    public int Port { get; init; } = 587;

    /// <summary>
    /// Ник.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Имя отправителя.
    /// </summary>
    public required string SenderName { get; init; }

    /// <summary>
    /// Почта отправителя.
    /// </summary>
    public required string SenderEmail { get; init; }
}