using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Infrastructure.Settings;

namespace Notification.Infrastructure.Services.Email;

/// <inheritdoc />
public class EmailService(
    IOptions<SmtpOptions> options,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpOptions _options = options.Value;

    /// <inheritdoc />
    public async Task SendAsync(string to, string subject, string body, CancellationToken ct)
    {
        logger.LogInformation("Sending email to {Email} with subject: {Subject}", to, subject);
        
        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port, SecureSocketOptions.StartTls, ct);
        await client.AuthenticateAsync(_options.Username, _options.Password, ct);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = FormatHtml(body) };

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);

        logger.LogInformation("Email sent successfully to {Email}", to);
    }

    private static string FormatHtml(string message)
    {
        return $$"""
                 <!DOCTYPE html>
                 <html>
                 <head>
                     <meta charset="utf-8">
                     <style>
                         body { font-family: Arial, sans-serif; padding: 20px; }
                         .content { max-width: 600px; margin: 0 auto; }
                     </style>
                 </head>
                 <body>
                     <div class="content">
                         <p>{{message}}</p>
                     </div>
                 </body>
                 </html>
                 """;
    }
}