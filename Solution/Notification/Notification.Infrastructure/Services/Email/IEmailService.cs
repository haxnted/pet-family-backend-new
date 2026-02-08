namespace Notification.Infrastructure.Services.Email;

/// <summary>
/// Сервис отправки email-уведомлений.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправляет email указанному получателю.
    /// </summary>
    /// <param name="to">Email получателя.</param>
    /// <param name="subject">Тема письма.</param>
    /// <param name="body">Тело письма.</param>
    /// <param name="ct">Токен отмены.</param>
    Task SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken ct);
}